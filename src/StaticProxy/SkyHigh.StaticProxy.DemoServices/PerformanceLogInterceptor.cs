using Microsoft.Extensions.Logging;
using SkyHigh.StaticProxy;
using System.Diagnostics;

namespace SkyHigh.StaticProxy.DemoServices
{
    public class PerformanceLogInterceptor<TInterface, TImplementation>(ILogger<PerformanceLogInterceptor<TInterface, TImplementation>> logger)
        : IInterceptor<TInterface, TImplementation>
        where TInterface : class
        where TImplementation : class, TInterface
    {
        private readonly ILogger _logger = logger;

        public void Intercept(IInterceptorContext<TInterface, TImplementation> context)
        {
            var stopWatch = Stopwatch.StartNew();

            try
            {
                context.Proceed();
            }
            finally
            {
                _logger.LogInformation("Sync PerformanceLogInterceptor: {ClassName}{MethodName} took {Duration} ms",
                    context.InterfaceMethodInfo.DeclaringType?.FullName,
                    context.InterfaceMethodInfo.Name,
                    stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task InterceptAsync(IInterceptorContext<TInterface, TImplementation> context)
        {
            var stopWatch = Stopwatch.StartNew();

            try
            {
                await context.ProceedAsync();
            }
            finally
            {
                _logger.LogInformation("Async PerformanceLogInterceptor: {ClassName}{MethodName} took {Duration} ms",
                    context.InterfaceMethodInfo.DeclaringType?.FullName,
                    context.InterfaceMethodInfo.Name,
                    stopWatch.ElapsedMilliseconds);
            }
        }
    }
}