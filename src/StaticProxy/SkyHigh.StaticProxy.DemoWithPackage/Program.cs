using Scalar.AspNetCore;
using SkyHigh.StaticProxy;
using SkyHigh.StaticProxy.Demo;
using SkyHigh.StaticProxy.Demo.Services;
using SkyHigh.StaticProxy.DemoServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddScopedWithInterceptors<IDemoService, DemoService>(typeof(LoggingInterceptor<,>));
builder.Services.AddSingletonWithInterceptors<IRemoteDemoService, RemoteDemoService>(typeof(LoggingInterceptor<,>), typeof(PerformanceLogInterceptor<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();