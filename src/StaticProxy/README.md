# SkyHigh.StaticProxy

SkyHigh.StaticProxy is a lightweight, high-performance .NET library that provides compile-time method interception through source generators. It allows you to implement Aspect-Oriented Programming (AOP) patterns without the runtime overhead of traditional dynamic proxies.

## Features

- **Compile-Time Proxy Generation**: Generate proxy classes at compile time using C# Source Generators
- **High Performance**: No runtime reflection overhead compared to dynamic proxies
- **Aspect-Oriented Programming**: Cleanly separate cross-cutting concerns such as logging, caching, and performance monitoring
- **DI Integration**: Seamless integration with Microsoft's Dependency Injection system
- **Multiple Interceptors**: Apply multiple interceptors to a single service
- **Async Support**: Full support for async methods with Task and ValueTask return types
- **Minimal Dependencies**: Only requires Microsoft.Extensions.DependencyInjection.Abstractions
- **Multi-Target**: Supports .NET Standard 2.0 and .NET 9.0

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package SkyHigh.StaticProxy
```

## Quick Start

### 1. Define Your Interface and Implementation

```csharp
public interface IMyService
{
    string GetData(string key);
    Task<int> GetCountAsync();
}

public class MyService : IMyService
{
    public string GetData(string key)
    {
        return $"Data for {key}";
    }

    public async Task<int> GetCountAsync()
    {
        await Task.Delay(100); // Simulate work
        return 42;
    }
}
```

### 2. Create an Interceptor

```csharp
public class LoggingInterceptor<TInterface, TImplementation> : IInterceptor<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private readonly ILogger _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor<TInterface, TImplementation>> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInterceptorContext<TInterface, TImplementation> context)
    {
        _logger.LogInformation($"Calling {context.InterfaceMethodInfo.Name}");
        context.Proceed();
        _logger.LogInformation($"Completed {context.InterfaceMethodInfo.Name}");
    }

    public async Task InterceptAsync(IInterceptorContext<TInterface, TImplementation> context)
    {
        _logger.LogInformation($"Calling async {context.InterfaceMethodInfo.Name}");
        await context.ProceedAsync();
        _logger.LogInformation($"Completed async {context.InterfaceMethodInfo.Name}");
    }
}
```

### 3. Register Services with Interceptors

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register service with interceptor
builder.Services.AddScopedWithInterceptors<IMyService, MyService>(typeof(LoggingInterceptor<,>));

// Multiple interceptors can be applied
builder.Services.AddSingletonWithInterceptors<IOtherService, OtherService>(
    typeof(LoggingInterceptor<,>), 
    typeof(PerformanceInterceptor<,>));
```

## How It Works

SkyHigh.StaticProxy uses C# Source Generators to analyze your code at compile time. When you register a service with interceptors, the SkyHigh.StaticProxy.Generator creates a strongly-typed proxy class that:

1. Implements the target interface
2. Wraps the actual implementation
3. Applies interceptors to each method call
4. Handles both synchronous and asynchronous methods

This approach provides the benefits of AOP without the performance penalties of runtime reflection or dynamic proxy generation.

## Interceptor Context

The `IInterceptorContext<TInterface, TImplementation>` provides key information and control:

- `InterfaceMethodInfo`: Information about the intercepted method
- `ImplementationMethodInfo`: Information about the implementation method
- `Parameters`: The parameters passed to the method
- `ReturnValue`: The return value (available after Proceed/ProceedAsync)
- `Implementation`: The actual implementation instance
- `Proceed()`: Calls the next interceptor or the implementation method
- `ProceedAsync()`: Async version of Proceed()

## Advanced Usage

### Creating a Performance Monitoring Interceptor

```csharp
public class PerformanceInterceptor<TInterface, TImplementation> : IInterceptor<TInterface, TImplementation>
    where TImplementation : class, TInterface
{
    private readonly ILogger _logger;

    public PerformanceInterceptor(ILogger<PerformanceInterceptor<TInterface, TImplementation>> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInterceptorContext<TInterface, TImplementation> context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            context.Proceed();
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation($"{context.InterfaceMethodInfo.Name} executed in {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    public async Task InterceptAsync(IInterceptorContext<TInterface, TImplementation> context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await context.ProceedAsync();
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation($"{context.InterfaceMethodInfo.Name} executed in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
