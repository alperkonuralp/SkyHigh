namespace SkyHigh.StaticProxy;

/// <summary>
/// Defines the contract for an interceptor.
/// </summary>
/// <typeparam name="TInterface">The interface type being intercepted.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
public interface IInterceptor<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    /// <summary>
    /// Intercepts a synchronous method call.
    /// </summary>
    /// <param name="context">The context of the interception.</param>
    void Intercept(IInterceptorContext<TInterface, TImplementation> context);

    /// <summary>
    /// Intercepts an asynchronous method call.
    /// </summary>
    /// <param name="context">The context of the interception.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InterceptAsync(IInterceptorContext<TInterface, TImplementation> context);
}
