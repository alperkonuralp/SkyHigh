using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SkyHigh.StaticProxy.Generator
{
    [Generator]
    public class ProxyGenerator : IIncrementalGenerator
    {
        // Method names to look for
        private static readonly string[] ProxyServiceMethods =
        [
            "AddScopedWithInterceptors",
            "AddTransientWithInterceptors",
            "AddSingletonWithInterceptors"
        ];

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register proxy service method invocations
            var methodCallsAndCompilation =
                context.CompilationProvider.Combine(
                    context.SyntaxProvider
                        .CreateSyntaxProvider(
                            predicate: static (s, _) => IsProxyServiceMethodCall(s),
                            transform: static (ctx, _) => GetProxyServiceMethodCall(ctx))
                        .Where(static m => m is not null)
                        .Collect());

            // Register source output for method-based generation
            context.RegisterSourceOutput(methodCallsAndCompilation,
                static (spc, source) => ExecuteMethodBasedGeneration(source.Left, source.Right, spc));
        }

        private static bool IsProxyServiceMethodCall(SyntaxNode node)
        {
            // Look for invocation expressions
            if (node is InvocationExpressionSyntax invocation && invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                // Get the method name
                string methodName = memberAccess.Name.Identifier.ValueText;

                // Check if it's one of the proxy service method names
                return ProxyServiceMethods.Contains(methodName);
            }

            return false;
        }

        private static InvocationExpressionSyntax? GetProxyServiceMethodCall(GeneratorSyntaxContext context)
        {
            // Cast the node to InvocationExpressionSyntax
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Get the semantic model
            var semanticModel = context.SemanticModel;

            // Get the symbol info for the method being called
            var symbolInfo = semanticModel.GetSymbolInfo(invocation);

            // If we have a method symbol, check if it's in the right namespace and has the right name
            if (symbolInfo.Symbol is IMethodSymbol methodSymbol)
            {
                string methodName = methodSymbol.Name;
                string containingNamespaceName = methodSymbol.ContainingNamespace?.ToDisplayString() ?? string.Empty;

                // Check if it's one of our target methods in the StaticProxy namespace
                if (containingNamespaceName == "SkyHigh.StaticProxy" && ProxyServiceMethods.Contains(methodName)
                    && invocation.Expression is MemberAccessExpressionSyntax memberAccess
                    && memberAccess.Name is GenericNameSyntax genericName
                    && genericName.TypeArgumentList.Arguments.Count >= 2)
                {
                    return invocation;
                }
            }

            return null;
        }

        private static void ExecuteMethodBasedGeneration(Compilation compilation, ImmutableArray<InvocationExpressionSyntax?> methodCalls, SourceProductionContext context)
        {
            if (methodCalls.IsEmpty)
                return;

            var processedTypes = new HashSet<string>(); // Track already processed types to avoid duplicates

            foreach (var methodCall in methodCalls.Where(m => m is not null).Select(m => m!))
            {
                // Get the semantic model for the method call
                var semanticModel = compilation.GetSemanticModel(methodCall.SyntaxTree);

                // Get the expression and extract the generic name syntax
                if (methodCall.Expression is MemberAccessExpressionSyntax memberAccess &&
                    memberAccess.Name is GenericNameSyntax genericName)
                {
                    var typeArguments = genericName.TypeArgumentList.Arguments;

                    // We need at least two type arguments: TInterface and TImplementation
                    if (typeArguments.Count < 2)
                    {
                        continue;
                    }

                    // Get the types
                    var interfaceTypeInfo = semanticModel.GetTypeInfo(typeArguments[0]);
                    var implementationTypeInfo = semanticModel.GetTypeInfo(typeArguments[1]);

                    // Check if both types are valid
                    if (interfaceTypeInfo.Type is null || implementationTypeInfo.Type is null)
                    {
                        continue;
                    }

                    var interfaceType = interfaceTypeInfo.Type;
                    var implementationType = implementationTypeInfo.Type;

                    // Generate a unique ID for this type combination
                    string typeKey = $"{implementationType.ToDisplayString()}";

                    // Only generate once for each implementation type
                    if (!processedTypes.Contains(typeKey))
                    {
                        processedTypes.Add(typeKey);

                        // Generate proxy class
                        GenerateProxyClass(context, interfaceType, implementationType);
                    }
                }
            }
        }

        private static void GenerateProxyClass(SourceProductionContext context, ITypeSymbol interfaceType, ITypeSymbol implementationType)
        {
            string implementationTypeName = implementationType.Name;
            string proxyClassName = $"{implementationTypeName}__Proxy";
            string implementationNamespace = implementationType.ContainingNamespace.ToDisplayString();

            // Get all the methods from the interface
            var interfaceMethods = GetAllInterfaceMethods(interfaceType);

            // Start building the proxy class
            var sb = new StringBuilder();

            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("// This code was generated by the SkyHigh.StaticProxy.Generator");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("#nullable enable");
            sb.AppendLine();

            // Add namespace
            sb.AppendLine($"namespace {implementationNamespace}");
            sb.AppendLine("{");

            // Start class declaration
            sb.AppendLine("  [System.CodeDom.Compiler.GeneratedCodeAttribute(\"SkyHigh.StaticProxy.Generator.ProxyGenerator\", \"1.0.0.0\")]");
            sb.AppendLine($"  internal sealed class {proxyClassName} : SkyHigh.StaticProxy.ProxyBase<{interfaceType.ToDisplayString()}, {implementationType.ToDisplayString()}>, {interfaceType.ToDisplayString()}");
            sb.AppendLine("  {");

            // Add constructor
            sb.AppendLine($"    public {proxyClassName}(");
            sb.AppendLine($"      {implementationType.ToDisplayString()} implementation,");
            sb.AppendLine($"      System.Collections.Generic.IEnumerable<SkyHigh.StaticProxy.IInterceptor<{interfaceType.ToDisplayString()}, {implementationType.ToDisplayString()}>> interceptors)");
            sb.AppendLine("       : base(implementation, interceptors)");
            sb.AppendLine("    {");
            sb.AppendLine("    }");

            // Generate method implementations
            foreach (var methodSymbol in interfaceMethods)
            {
                sb.AppendLine();
                GenerateProxyMethod(sb, methodSymbol);
            }

            // Close class and namespace
            sb.AppendLine("  }");
            sb.AppendLine("}");

            // Add the source file
            context.AddSource($"{proxyClassName}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static void GenerateProxyMethod(StringBuilder sb, IMethodSymbol method)
        {
            // Get return type
            string returnType = method.ReturnType.ToDisplayString();

            // Get method name
            string methodName = method.Name;

            // Get method parameters
            string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}"));

            // Get parameter names for calling the implementation
            string parameterNames = string.Join(", ", method.Parameters.Select(p => p.Name));

            // Is the method async?
            bool isAsync = method.ReturnType.Name == "Task" || method.ReturnType.Name == "ValueTask" ||
                          (method.ReturnType is INamedTypeSymbol namedType &&
                          (namedType.Name == "Task" || namedType.Name == "ValueTask"));

            // Start method implementation
            sb.AppendLine($"    public {returnType} {methodName}({parameters})");
            sb.AppendLine("    {");

            if (isAsync)
            {
                if (returnType == "Task" || returnType == "System.Threading.Tasks.Task")
                {
                    sb.AppendLine("      return RunForActionAsync(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
                else if (returnType == "ValueTask" || returnType == "System.Threading.Tasks.ValueTask")
                {
                    sb.AppendLine("      return RunForValueActionAsync(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
                else if (returnType.StartsWith("ValueTask") || returnType.StartsWith("System.Threading.Tasks.ValueTask"))
                {
                    sb.AppendLine("      return RunForValueFunctionAsync(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
                else
                {
                    sb.AppendLine("      return RunForFunctionAsync(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
            }
            else
            {
                if (returnType == "void")
                {
                    sb.AppendLine("      RunForAction(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
                else
                {
                    sb.AppendLine("      return RunForFunction(");
                    sb.AppendLine($"        i => i.{methodName}({parameterNames}), // interface method");
                    sb.AppendLine($"        c => c.{methodName}({parameterNames}), // class method");
                    sb.AppendLine($"        m => m.{methodName}({parameterNames}), // runner method");
                    sb.AppendLine($"        new object?[]{{ {parameterNames} }} // parameters");
                    sb.AppendLine("      );");
                }
            }

            sb.AppendLine("    }");
        }

        private static IEnumerable<IMethodSymbol> GetAllInterfaceMethods(ITypeSymbol interfaceType)
        {
            var result = new List<IMethodSymbol>();

            // Get directly declared methods
            if (interfaceType is INamedTypeSymbol namedType)
            {
                foreach (var member in namedType.GetMembers())
                {
                    if (member is IMethodSymbol methodSymbol && !methodSymbol.IsStatic)
                    {
                        result.Add(methodSymbol);
                    }
                }

                // Get methods from base interfaces
                foreach (var baseInterface in namedType.AllInterfaces)
                {
                    foreach (var member in baseInterface.GetMembers())
                    {
                        if (member is IMethodSymbol methodSymbol && !methodSymbol.IsStatic)
                        {
                            result.Add(methodSymbol);
                        }
                    }
                }
            }

            return result;
        }
    }
}
