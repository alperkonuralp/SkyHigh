using System.Reflection;

namespace SkyHigh.StaticProxy.Contexts;

/// <summary>
/// Provides context for intercepting asynchronous void methods that use ValueTask.
/// </summary>
/// <typeparam name="TInterface">The interface type being intercepted.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the interface.</typeparam>
/// <param name="interceptors">The list of interceptors that will process the method call.</param>
/// <param name="implementation">The actual implementation instance.</param>
/// <param name="interfaceMethodInfo">Metadata for the interface method being called.</param>
/// <param name="implementationMethodInfo">Metadata for the implementation method being called.</param>
/// <param name="parameters">The parameters passed to the method.</param>
/// <param name="implementationRunner">Function that executes the implementation method.</param>
internal class ValueAsyncInterceptorContext<TInterface, TImplementation>(
    IReadOnlyList<IInterceptor<TInterface, TImplementation>> interceptors,
    TImplementation implementation,
    MethodInfo interfaceMethodInfo,
    MethodInfo implementationMethodInfo,
    IReadOnlyList<object?> parameters,
    Func<ValueTask> implementationRunner)
    : IInterceptorContext<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private int runningIndex = -1;

    /// <inheritdoc />
    public TInterface InterfaceRef => ImplementationRef;

    /// <inheritdoc />
    public TImplementation ImplementationRef => implementation;

    /// <inheritdoc />
    public MethodInfo InterfaceMethodInfo => interfaceMethodInfo;

    /// <inheritdoc />
    public MethodInfo ImplementationMethodInfo => implementationMethodInfo;

    /// <inheritdoc />
    public IReadOnlyList<object?> Parameters => parameters;

    /// <summary>
    /// Gets a value indicating whether the intercepted method is asynchronous.
    /// </summary>
    public bool IsAsync => true;

    /// <inheritdoc />
    public bool IsReturnVoid => true;

    /// <summary>
    /// Gets or sets the return value from the method execution.
    /// </summary>
    public object? ReturnValue { get; set; } = null;

    /// <summary>
    /// Synchronously proceeds with the interception pipeline.
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown as this is not supported for ValueTask methods.</exception>
    public void Proceed()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Asynchronously proceeds with the interception pipeline, calling the next interceptor or the implementation method.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProceedAsync()
    {
        runningIndex++;
        if (runningIndex < interceptors.Count)
        {
            await interceptors[runningIndex].InterceptAsync(this);
        }
        else if (runningIndex == interceptors.Count)
        {
            await implementationRunner();
        }
    }
}

/// <summary>
/// Provides context for intercepting asynchronous methods with return values that use ValueTask.
/// </summary>
/// <typeparam name="TInterface">The interface type being intercepted.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the interface.</typeparam>
/// <typeparam name="TResult">The return type of the intercepted method.</typeparam>
/// <param name="interceptors">The list of interceptors that will process the method call.</param>
/// <param name="implementation">The actual implementation instance.</param>
/// <param name="interfaceMethodInfo">Metadata for the interface method being called.</param>
/// <param name="implementationMethodInfo">Metadata for the implementation method being called.</param>
/// <param name="parameters">The parameters passed to the method.</param>
/// <param name="implementationRunner">Function that executes the implementation method and returns its result.</param>
internal class ValueAsyncInterceptorContext<TInterface, TImplementation, TResult>(
    IReadOnlyList<IInterceptor<TInterface, TImplementation>> interceptors,
    TImplementation implementation,
    MethodInfo interfaceMethodInfo,
    MethodInfo implementationMethodInfo,
    IReadOnlyList<object?> parameters,
    Func<ValueTask<TResult>> implementationRunner)
    : IInterceptorContext<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private int runningIndex = -1;

    /// <inheritdoc />
    public TInterface InterfaceRef => ImplementationRef;

    /// <inheritdoc />
    public TImplementation ImplementationRef => implementation;

    /// <inheritdoc />
    public MethodInfo InterfaceMethodInfo => interfaceMethodInfo;

    /// <inheritdoc />
    public MethodInfo ImplementationMethodInfo => implementationMethodInfo;

    /// <inheritdoc />
    public IReadOnlyList<object?> Parameters => parameters;

    /// <summary>
    /// Gets a value indicating whether the intercepted method is asynchronous.
    /// </summary>
    public bool IsAsync => true;

    /// <summary>
    /// Gets a value indicating whether the intercepted method returns void.
    /// </summary>
    public bool IsReturnVoid => false;

    /// <summary>
    /// Gets or sets the return value from the method execution.
    /// </summary>
    public object? ReturnValue { get; set; } = null;

    /// <summary>
    /// Synchronously proceeds with the interception pipeline.
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown as this is not supported for ValueTask methods.</exception>
    public void Proceed()
    {
        throw new NotImplementedException("This method is not supported for asynchronous methods.");
    }

    /// <summary>
    /// Asynchronously proceeds with the interception pipeline, calling the next interceptor or the implementation method.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProceedAsync()
    {
        runningIndex++;
        if (runningIndex < interceptors.Count)
        {
            await interceptors[runningIndex].InterceptAsync(this);
        }
        else if (runningIndex == interceptors.Count)
        {
            ReturnValue = await implementationRunner();
        }
    }
}