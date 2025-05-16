using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyHigh.StaticProxy.Demo.Services;

namespace SkyHigh.StaticProxy.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private static readonly List<DemoDto> list =
    [
        new DemoDto(1, "Alice", DateTimeOffset.UtcNow.AddDays(-10)),
        new DemoDto(2, "Bob", DateTimeOffset.UtcNow.AddDays(-9)),
        new DemoDto(3, "Charlie", DateTimeOffset.UtcNow.AddDays(-8)),
        new DemoDto(4, "Diana", DateTimeOffset.UtcNow.AddDays(-7)),
        new DemoDto(5, "Eve", DateTimeOffset.UtcNow.AddDays(-6)),
        new DemoDto(6, "Frank", DateTimeOffset.UtcNow.AddDays(-5)),
        new DemoDto(7, "Grace", DateTimeOffset.UtcNow.AddDays(-4)),
        new DemoDto(8, "Hank", DateTimeOffset.UtcNow.AddDays(-3)),
        new DemoDto(9, "Ivy", DateTimeOffset.UtcNow.AddDays(-2)),
        new DemoDto(10, "Jack", DateTimeOffset.UtcNow.AddDays(-1))
    ];

    [HttpGet("[action]")]
    public IActionResult Action0([FromServices] IDemoService demoService)
    {
        demoService.Action0();
        return Ok(list);
    }

    [HttpGet("[action]")]
    public IActionResult Action1([FromServices] IDemoService demoService)
    {
        demoService.Action1("Alper");
        return Ok(list);
    }

    [HttpGet("[action]")]
    public IActionResult Action2([FromServices] IDemoService demoService)
    {
        demoService.Action2("Alper", DateTimeOffset.Now);
        return Ok(list);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncAction0([FromServices] IDemoService demoService)
    {
        await demoService.AsyncAction0();
        return Ok(list);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncAction1([FromServices] IDemoService demoService)
    {
        await demoService.AsyncAction1("Alper");
        return Ok(list);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncAction2([FromServices] IDemoService demoService)
    {
        await demoService.AsyncAction2("Alper", DateTimeOffset.Now);
        return Ok(list);
    }

    [HttpGet("[action]")]
    public IActionResult Function0([FromServices] IDemoService demoService)
    {
        var result = demoService.Function0();
        return Ok(new { list, result });
    }

    [HttpGet("[action]")]
    public IActionResult Function1([FromServices] IDemoService demoService)
    {
        var result = demoService.Function1("Alper");
        return Ok(new { list, result });
    }

    [HttpGet("[action]")]
    public IActionResult Function2([FromServices] IDemoService demoService)
    {
        var result = demoService.Function2("Alper", DateTimeOffset.Now);
        return Ok(new { list, result });
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncFunction0([FromServices] IDemoService demoService)
    {
        var result = await demoService.AsyncFunction0();
        return Ok(new { list, result });
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncFunction1([FromServices] IDemoService demoService)
    {
        var result = await demoService.AsyncFunction1("Alper");
        return Ok(new { list, result });
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AsyncFunction2([FromServices] IDemoService demoService)
    {
        var result = await demoService.AsyncFunction2("Alper", DateTimeOffset.Now);
        return Ok(new { list, result });
    }

    public record DemoDto(int Id, string Name, DateTimeOffset CreatedAt);
}