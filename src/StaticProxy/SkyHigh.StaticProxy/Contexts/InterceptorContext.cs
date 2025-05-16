using System.Reflection;

namespace SkyHigh.StaticProxy.Contexts;

/// <summary>
/// Represents the context of an interception.
/// </summary>
/// <typeparam name="TInterface">The interface type being proxied.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="InterceptorContext{TInterface, TImplementation}"/> class.
/// </remarks>
/// <param name="interceptors">The list of interceptors.</param>
/// <param name="implementation">The implementation instance.</param>
/// <param name="interfaceMethodInfo">The <see cref="MethodInfo"/> of the interface method.</param>
/// <param name="implementationMethodInfo">The <see cref="MethodInfo"/> of the implementation method.</param>
/// <param name="parameters">The parameters of the method call.</param>
/// <param name="implementationRunner">A function that runs the actual implementation.</param>
internal class InterceptorContext<TInterface, TImplementation>(
    IReadOnlyList<IInterceptor<TInterface, TImplementation>> interceptors,
    TImplementation implementation,
    MethodInfo interfaceMethodInfo,
    MethodInfo implementationMethodInfo,
    IReadOnlyList<object?> parameters,
    Action implementationRunner)
    : IInterceptorContext<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private int runningIndex = -1;

    /// <inheritdoc />
    public bool IsAsync => false;

    /// <inheritdoc />
    public bool IsReturnVoid => true;

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

    /// <inheritdoc />
    public object? ReturnValue { get; set; } = null;

    /// <inheritdoc />
    public void Proceed()
    {
        runningIndex++;

        if (runningIndex < interceptors.Count)
        {
            interceptors[runningIndex].Intercept(this);
        }
        else if (runningIndex == interceptors.Count)
        {
            implementationRunner();
        }
    }

    /// <inheritdoc />
    public Task ProceedAsync()
    {
        throw new NotImplementedException("This method is not supported for asynchronous methods.");
    }
}

/// <summary>
/// Represents the context of an interception for methods with return values.
/// </summary>
/// <typeparam name="TInterface">The interface type being proxied.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
/// <typeparam name="TResult">The return type of the intercepted method.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="InterceptorContext{TInterface, TImplementation, TResult}"/> class.
/// </remarks>
/// <param name="interceptors">The list of interceptors.</param>
/// <param name="implementation">The implementation instance.</param>
/// <param name="interfaceMethodInfo">The <see cref="MethodInfo"/> of the interface method.</param>
/// <param name="implementationMethodInfo">The <see cref="MethodInfo"/> of the implementation method.</param>
/// <param name="parameters">The parameters of the method call.</param>
/// <param name="implementationRunner">A function that runs the actual implementation and returns its result.</param>
internal class InterceptorContext<TInterface, TImplementation, TResult>(
    IReadOnlyList<IInterceptor<TInterface, TImplementation>> interceptors,
    TImplementation implementation,
    MethodInfo interfaceMethodInfo,
    MethodInfo implementationMethodInfo,
    IReadOnlyList<object?> parameters,
    Func<TResult> implementationRunner)
    : IInterceptorContext<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private int runningIndex = -1;

    /// <inheritdoc />
    public bool IsAsync => false;

    /// <inheritdoc />
    public bool IsReturnVoid => false;

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

    /// <inheritdoc />
    public object? ReturnValue { get; set; } = null;

    /// <inheritdoc />
    public void Proceed()
    {
        runningIndex++;
        if (runningIndex < interceptors.Count)
        {
            interceptors[runningIndex].Intercept(this);
        }
        else if (runningIndex == interceptors.Count)
        {
            ReturnValue = implementationRunner();
        }
    }

    /// <inheritdoc />
    public Task ProceedAsync()
    {
        throw new NotImplementedException("This method is not supported for synchronous methods.");
    }
}