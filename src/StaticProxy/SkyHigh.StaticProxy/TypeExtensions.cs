using System.Linq.Expressions;
using System.Reflection;

namespace SkyHigh.StaticProxy;

/// <summary>
/// Provides extension methods for <see cref="Type"/>.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is an asynchronous type (Task or Task&lt;T&gt;).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the specified type is an asynchronous type; otherwise, <c>false</c>.</returns>
    public static bool IsAsyncType(this Type type)
    {
        return type == typeof(Task) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
    }

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> for the specified method name and parameters.
    /// </summary>
    /// <typeparam name="T">The type to get the method from.</typeparam>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterCount">The number of parameters the method has.</param>
    /// <param name="isAsyncType">A value indicating whether the method is an asynchronous type.</param>
    /// <param name="genericParameterCount">The number of generic parameters the method has.</param>
    /// <param name="advantageFilter">An optional filter to apply if multiple methods are found.</param>
    /// <returns>The <see cref="MethodInfo"/> for the specified method.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the method is not found or if multiple methods are found and no advantage filter is provided.</exception>
    public static MethodInfo GetMethodInfo<T>(string name, int parameterCount = 0, bool isAsyncType = false, int genericParameterCount = 0, Func<IEnumerable<MethodInfo>, IEnumerable<MethodInfo>>? advantageFilter = null)
    {
        var n = name;
        if (!name.Contains('`') && genericParameterCount > 0)
        {
            n = name + "`" + genericParameterCount;
        }
        var t = typeof(T);
        var methodInfos = (isAsyncType
            ? t.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name == n && x.ReturnType.IsAsyncType())
            : t.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name == n))
            .ToArray();

        if (methodInfos.Length == 0)
        {
            throw new InvalidOperationException($"Method {n} not found.");
        }
        if (methodInfos.Length == 1)
        {
            return methodInfos[0];
        }
        if (parameterCount == 0)
            return methodInfos.FirstOrDefault(x => x.GetParameters().Length == 0) ?? throw new InvalidOperationException($"Method {n} not found.");

        if (advantageFilter == null)
        {
            throw new InvalidOperationException($"Method {n} has more then one instance.");
        }

        var mt2 = advantageFilter(methodInfos.Where(x => x.GetParameters().Length == parameterCount)).ToArray();

        if (mt2.Length == 0)
        {
            throw new InvalidOperationException($"Method {n} not found.");
        }
        if (mt2.Length == 1)
        {
            return mt2[0];
        }

        throw new InvalidOperationException($"Method {n} has more then one instance.");
    }

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> from an expression.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="expr">The expression to get the method from.</param>
    /// <returns>The <see cref="MethodInfo"/> from the expression.</returns>
    /// <exception cref="ArgumentException">Thrown if the expression is not a method call.</exception>
    public static MethodInfo GetMethodInfo<TService>(this Expression<Action<TService>> expr)
    {
        if (expr.Body is MethodCallExpression call)
            return call.Method;
        throw new ArgumentException("Expression must be a method call", nameof(expr));
    }

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> from an expression.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="expr">The expression to get the method from.</param>
    /// <returns>The <see cref="MethodInfo"/> from the expression.</returns>
    /// <exception cref="ArgumentException">Thrown if the expression is not a method call.</exception>
    public static MethodInfo GetMethodInfo<TService, TResult>(this Expression<Func<TService, TResult>> expr)
    {
        if (expr.Body is MethodCallExpression call)
            return call.Method;
        throw new ArgumentException("Expression must be a method call", nameof(expr));
    }
}