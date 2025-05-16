namespace SkyHigh.StaticProxy.DemoServices;

public interface IRemoteDemoService
{
    void Action0();

    Task AsyncAction1(string name);

    string AsyncFunction2(string name, DateTimeOffset date);

    string Function0();

    ValueTask ValueTaskAction2(string name, DateTimeOffset date);

    ValueTask<string> ValueTaskFunction0();
}