namespace SkyHigh.StaticProxy.DemoServices;

internal class InternalRemoteDemoService : IInternalRemoteDemoService
{
    public string GetString(string str)
    {
        return str;
    }

    public Task<string> GetStringAsync(string str)
    {
        return Task.FromResult(str);
    }

    public ValueTask<string> GetStringValueTask(string str)
    {
        return new ValueTask<string>(str);
    }
}
