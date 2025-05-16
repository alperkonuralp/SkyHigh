using Microsoft.Extensions.DependencyInjection;
using SkyHigh.StaticProxy;

namespace SkyHigh.StaticProxy.DemoServices;

public static class InternalServiceRegisterer
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransientWithInterceptors<IInternalRemoteDemoService, InternalRemoteDemoService>(typeof(PerformanceLogInterceptor<,>));
    }
}