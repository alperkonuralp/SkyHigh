namespace SkyHigh.StaticProxy.Demo;

public class LoggingInterceptor<TInterface, TImplementation> : IInterceptor<TInterface, TImplementation>
        where TImplementation : class, TInterface

{
    public void Intercept(IInterceptorContext<TInterface, TImplementation> context)
    {
        Console.WriteLine($"Intercepting {context.InterfaceMethodInfo.Name} with parameters: {string.Join(", ", context.Parameters)}");
        context.Proceed();
        Console.WriteLine($"Return value: {context.ReturnValue}");
    }

    public async Task InterceptAsync(IInterceptorContext<TInterface, TImplementation> context)
    {
        Console.WriteLine($"Intercepting {context.InterfaceMethodInfo.Name} with parameters: {string.Join(", ", context.Parameters)}");
        await context.ProceedAsync();
        Console.WriteLine($"Return value: {context.ReturnValue}");
    }
}