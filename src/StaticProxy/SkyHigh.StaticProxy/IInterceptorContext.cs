using System.Reflection;

namespace SkyHigh.StaticProxy;

/// <summary>
/// Defines the context for an interceptor.
/// </summary>
/// <typeparam name="TInterface">The type of the interface being intercepted.</typeparam>
/// <typeparam name="TImplementation">The type of the implementation being intercepted.</typeparam>
public interface IInterceptorContext<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    /// <summary>
    /// Gets a value indicating whether the intercepted method is asynchronous.
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// Gets a reference to the interface instance.
    /// </summary>
    TInterface InterfaceRef { get; }

    /// <summary>
    /// Gets a reference to the implementation instance.
    /// </summary>
    TImplementation ImplementationRef { get; }

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> for the interface method being intercepted.
    /// </summary>
    MethodInfo InterfaceMethodInfo { get; }

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> for the implementation method being intercepted.
    /// </summary>
    MethodInfo ImplementationMethodInfo { get; }

    /// <summary>
    /// Gets the parameters passed to the intercepted method.
    /// </summary>
    IReadOnlyList<object?> Parameters { get; }

    /// <summary>
    /// Gets or sets the return value of the intercepted method.
    /// </summary>
    object? ReturnValue { get; set; }
    /// <summary>
    /// Gets a value indicating whether the intercepted method has a void return type.
    /// </summary>
    bool IsReturnVoid { get; }

    /// <summary>
    /// Proceeds to the next interceptor in the chain, or to the actual implementation if this is the last interceptor.
    /// </summary>
    void Proceed();

    /// <summary>
    /// Proceeds asynchronously to the next interceptor in the chain, or to the actual implementation if this is the last interceptor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProceedAsync();
}