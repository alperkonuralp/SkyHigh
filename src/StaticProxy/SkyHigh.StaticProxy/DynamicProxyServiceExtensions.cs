using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SkyHigh.StaticProxy;

/// <summary>
/// Extension methods for setting up dynamic proxy services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DynamicProxyServiceExtensions
{
    /// <summary>
    /// Adds a transient service of the type specified in <typeparamref name="TInterface"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified <see cref="IServiceCollection"/>. Interceptors can be added to the proxy.
    /// </summary>
    /// <typeparam name="TInterface">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="interceptorTypes">The types of the interceptors to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddTransientWithInterceptors<TInterface, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddTransient<TImplementation>();
        Type proxyClassType = Prepare<TInterface, TImplementation>(services, interceptorTypes);

        services.AddTransient(typeof(TInterface), proxyClassType);
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in <typeparamref name="TInterface"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified <see cref="IServiceCollection"/>. Interceptors can be added to the proxy.
    /// </summary>
    /// <typeparam name="TInterface">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="interceptorTypes">The types of the interceptors to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedWithInterceptors<TInterface, TImplementation>(
        this IServiceCollection services,
        params Type[] interceptorTypes)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>();
        Type proxyClassType = Prepare<TInterface, TImplementation>(services, interceptorTypes);

        services.AddScoped(typeof(TInterface), proxyClassType);
        return services;
    }

    /// <summary>
    /// Adds a singleton service of the type specified in <typeparamref name="TInterface"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified <see cref="IServiceCollection"/>. Interceptors can be added to the proxy.
    /// </summary>
    /// <typeparam name="TInterface">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="interceptorTypes">The types of the interceptors to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddSingletonWithInterceptors<TInterface, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddSingleton<TImplementation>();
        Type proxyClassType = Prepare<TInterface, TImplementation>(services, interceptorTypes);

        services.AddSingleton(typeof(TInterface), proxyClassType);
        return services;
    }

    /// <summary>
    /// Prepares the proxy class type and registers the interceptors for the given service types.
    /// </summary>
    /// <typeparam name="TInterface">The interface type of the service.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the interceptors to.</param>
    /// <param name="interceptorTypes">The types of the interceptors to register.</param>
    /// <returns>The generated proxy class type that implements the interface.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the proxy class cannot be found.</exception>
    private static Type Prepare<TInterface, TImplementation>(IServiceCollection services, Type[] interceptorTypes)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        var proxyType = typeof(TImplementation).Name + "__Proxy";
        var proxyClassType = typeof(TImplementation).Assembly.GetTypes().FirstOrDefault(t => t.Name == proxyType)
            ?? Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == proxyType)
            ?? Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(t => t.Name == proxyType)
            ?? Assembly.GetEntryAssembly()?.GetTypes().FirstOrDefault(t => t.Name == proxyType)
            ?? throw new InvalidOperationException($"Proxy class {proxyType} not found.");

        var intList = interceptorTypes
            .Select(x => x.IsGenericTypeDefinition ? x : x.GetGenericTypeDefinition())
            .Select(x => x.MakeGenericType(typeof(TInterface), typeof(TImplementation)))
            .ToList();

        var interceptorType = typeof(IInterceptor<TInterface, TImplementation>);
        foreach (var intType in intList)
        {
            services.AddTransient(interceptorType, intType);
        }

        return proxyClassType;
    }
}