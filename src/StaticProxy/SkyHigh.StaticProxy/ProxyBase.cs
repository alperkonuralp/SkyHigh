#nullable disable

using SkyHigh.StaticProxy.Contexts;
using System.Linq.Expressions;

namespace SkyHigh.StaticProxy;

/// <summary>
/// Base class for generated proxy types.
/// </summary>
/// <typeparam name="TInterface">The interface type being proxied.</typeparam>
/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
/// <param name="implementation">The instance of the implementation.</param>
/// <param name="interceptors">The interceptors to be applied.</param>
public class ProxyBase<TInterface, TImplementation>(TImplementation implementation, IEnumerable<IInterceptor<TInterface, TImplementation>> interceptors)
    where TInterface : class
    where TImplementation : class, TInterface
{
    /// <summary>
    /// The instance of the implementation being proxied.
    /// </summary>
    protected readonly TImplementation _implementation = implementation;

    /// <summary>
    /// The list of interceptors to be applied.
    /// </summary>
    protected readonly IReadOnlyList<IInterceptor<TInterface, TImplementation>> _interceptors = [.. interceptors];

    /// <summary>
    /// Executes a method that returns a value.
    /// </summary>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">A func that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    /// <returns>The result of the method execution.</returns>
    protected TResult RunForFunction<TResult>(
        Expression<Func<TInterface, TResult>> interfaceMethodExpression,
        Expression<Func<TImplementation, TResult>> implementationMethodExpression,
        Func<TImplementation, TResult> implementationMethod,
        IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            return implementationMethod(_implementation);
        }

        var context = new InterceptorContext<TInterface, TImplementation, TResult>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters: parameters,
            implementationRunner: () => implementationMethod(_implementation));

        context.Proceed();

        return context.ReturnValue is TResult result ? result : default;
    }

    /// <summary>
    /// Executes an asynchronous method that returns a value.
    /// </summary>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">A func that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the method execution.</returns>
    protected async Task<TResult> RunForFunctionAsync<TResult>(
        Expression<Func<TInterface, Task<TResult>>> interfaceMethodExpression,
        Expression<Func<TImplementation, Task<TResult>>> implementationMethodExpression,
        Func<TImplementation, Task<TResult>> implementationMethod,
        IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            return await implementationMethod(_implementation);
        }

        var context = new AsyncInterceptorContext<TInterface, TImplementation, TResult>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters: parameters,
            implementationRunner: () => implementationMethod(_implementation));

        await context.ProceedAsync();

        return context.ReturnValue is TResult result ? result : default;
    }

    /// <summary>
    /// Executes an asynchronous method that returns a value as a ValueTask.
    /// </summary>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">A func that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    /// <returns>A ValueTask representing the asynchronous operation, with the result of the method execution.</returns>
    protected async ValueTask<TResult> RunForValueFunctionAsync<TResult>(
        Expression<Func<TInterface, ValueTask<TResult>>> interfaceMethodExpression,
        Expression<Func<TImplementation, ValueTask<TResult>>> implementationMethodExpression,
        Func<TImplementation, ValueTask<TResult>> implementationMethod,
        IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            return await implementationMethod(_implementation);
        }

        var context = new ValueAsyncInterceptorContext<TInterface, TImplementation, TResult>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters: parameters,
            implementationRunner: () => implementationMethod(_implementation));

        await context.ProceedAsync();

        return context.ReturnValue is TResult result ? result : default;
    }

    /// <summary>
    /// Executes a method that does not return a value.
    /// </summary>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">An action that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    protected void RunForAction(
        Expression<Action<TInterface>> interfaceMethodExpression,
        Expression<Action<TImplementation>> implementationMethodExpression,
        Action<TImplementation> implementationMethod,
        IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            implementationMethod(_implementation);
            return;
        }

        var context = new InterceptorContext<TInterface, TImplementation>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters,
            () => implementationMethod(_implementation));

        context.Proceed();
    }

    /// <summary>
    /// Executes an asynchronous method that does not return a value.
    /// </summary>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">A func that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task RunForActionAsync(
        Expression<Func<TImplementation, Task>> interfaceMethodExpression,
        Expression<Func<TImplementation, Task>> implementationMethodExpression,
        Func<TImplementation, Task> implementationMethod,
        IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            await implementationMethod(_implementation);
            return;
        }

        var context = new AsyncInterceptorContext<TInterface, TImplementation>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters,
            () => implementationMethod(_implementation));

        await context.ProceedAsync();
    }

    /// <summary>
    /// Executes an asynchronous method that does not return a value as a ValueTask.
    /// </summary>
    /// <param name="interfaceMethodExpression">Expression representing the interface method.</param>
    /// <param name="implementationMethodExpression">Expression representing the implementation method.</param>
    /// <param name="implementationMethod">A func that calls the actual implementation method.</param>
    /// <param name="parameters">The parameters passed to the method.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async ValueTask RunForValueActionAsync(
       Expression<Func<TImplementation, ValueTask>> interfaceMethodExpression,
       Expression<Func<TImplementation, ValueTask>> implementationMethodExpression,
       Func<TImplementation, ValueTask> implementationMethod,
       IReadOnlyList<object> parameters)
    {
        if (_interceptors.Count == 0)
        {
            await implementationMethod(_implementation);
            return;
        }

        var context = new ValueAsyncInterceptorContext<TInterface, TImplementation>(
            _interceptors,
            _implementation,
            interfaceMethodExpression.GetMethodInfo(),
            implementationMethodExpression.GetMethodInfo(),
            parameters,
            () => implementationMethod(_implementation));

        await context.ProceedAsync();
    }
}