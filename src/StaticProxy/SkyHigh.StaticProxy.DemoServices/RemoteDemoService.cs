namespace SkyHigh.StaticProxy.DemoServices;

public class RemoteDemoService : IRemoteDemoService
{
    public void Action0()
    {
        // Implementation of Action0
        Console.WriteLine("Action0 executed");
    }

    public Task AsyncAction1(string name)
    {
        // Implementation of AsyncAction1
        Console.WriteLine($"AsyncAction1 executed with name: {name}");
        return Task.CompletedTask;
    }

    public string Function0()
    {
        // Implementation of Function0
        Console.WriteLine("Function0 executed");
        return "Function0 result";
    }

    public string AsyncFunction2(string name, DateTimeOffset date)
    {
        // Implementation of AsyncFunction2
        Console.WriteLine($"AsyncFunction2 executed with name: {name} and date: {date}");
        return "AsyncFunction2 result";
    }

    public ValueTask ValueTaskAction2(string name, DateTimeOffset date)
    {
        // Implementation of ValueTaskAction2
        Console.WriteLine($"ValueTaskAction2 executed with name: {name} and date: {date}");
        return new ValueTask(Task.CompletedTask);
    }

    public ValueTask<string> ValueTaskFunction0()
    {
        // Implementation of ValueTaskFunction0
        Console.WriteLine("ValueTaskFunction0 executed");
        return new ValueTask<string>("ValueTaskFunction0 result");
    }
}