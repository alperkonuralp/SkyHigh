namespace SkyHigh.StaticProxy.DemoServices;

public interface IInternalRemoteDemoService
{
    string GetString(string str);

    Task<string> GetStringAsync(string str);

    ValueTask<string> GetStringValueTask(string str);
}