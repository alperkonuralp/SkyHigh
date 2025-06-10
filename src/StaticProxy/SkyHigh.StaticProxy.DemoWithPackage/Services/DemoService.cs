namespace SkyHigh.StaticProxy.Demo.Services;

public class DemoService : IDemoService
{
    // AOP : Aspect Oriented Programming

    public void Action0()
    {
        Console.WriteLine("Action 0 Demo");
    }

    public void Action1(string name)
    {
        Console.WriteLine($"Action 1 Demo : {name}");
    }

    public void Action2(string name, DateTimeOffset date)
    {
        Console.WriteLine($"Action 2 Demo : {name} , {date}");
    }

    public Task AsyncAction0()
    {
        Console.WriteLine("Async Action 0 Demo");
        return Task.CompletedTask;
    }

    public Task AsyncAction1(string name)
    {
        Console.WriteLine($"Async Action 1 Demo : {name}");
        return Task.CompletedTask;
    }

    public Task AsyncAction2(string name, DateTimeOffset date)
    {
        Console.WriteLine($"Async Action 2 Demo : {name} , {date}");
        return Task.CompletedTask;
    }

    public string Function0()
    {
        const string c = "Function 0 Demo";
        Console.WriteLine(c);
        return c;
    }

    public string Function1(string name)
    {
        var c = $"Function 1 Demo : {name}";
        Console.WriteLine(c);
        return c;
    }

    public string Function2(string name, DateTimeOffset date)
    {
        var c = $"Function 2 Demo : {name} , {date}";
        Console.WriteLine(c);
        return c;
    }

    public Task<string> AsyncFunction0()
    {
        const string c = "Async Function 0 Demo";
        Console.WriteLine(c);
        return Task.FromResult(c);
    }

    public Task<string> AsyncFunction1(string name)
    {
        var c = $"Async Function 1 Demo : {name}";
        Console.WriteLine(c);
        return Task.FromResult(c);
    }

    public Task<string> AsyncFunction2(string name, DateTimeOffset date)
    {
        var c = $"Async Function 2 Demo : {name} , {date}";
        Console.WriteLine(c);
        return Task.FromResult(c);
    }

    public ValueTask<string> ValueTaskFunction0()
    {
        const string c = "ValueTask Function 0 Demo";
        Console.WriteLine(c);
        return new ValueTask<string>(c);
    }

    public ValueTask<string> ValueTaskFunction1(string name)
    {
        var c = $"ValueTask Function 1 Demo : {name}";
        Console.WriteLine(c);
        return new ValueTask<string>(c);
    }

    public ValueTask<string> ValueTaskFunction2(string name, DateTimeOffset date)
    {
        var c = $"ValueTask Function 2 Demo : {name} , {date}";
        Console.WriteLine(c);
        return new ValueTask<string>(c);
    }
}