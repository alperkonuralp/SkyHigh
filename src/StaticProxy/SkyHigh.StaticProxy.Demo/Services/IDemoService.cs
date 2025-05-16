namespace SkyHigh.StaticProxy.Demo.Services
{
    public interface IDemoService
    {
        void Action0();

        void Action1(string name);

        void Action2(string name, DateTimeOffset date);

        Task AsyncAction0();

        Task AsyncAction1(string name);

        Task AsyncAction2(string name, DateTimeOffset date);

        Task<string> AsyncFunction0();

        Task<string> AsyncFunction1(string name);

        Task<string> AsyncFunction2(string name, DateTimeOffset date);

        string Function0();

        string Function1(string name);

        string Function2(string name, DateTimeOffset date);

        ValueTask<string> ValueTaskFunction0();

        ValueTask<string> ValueTaskFunction1(string name);

        ValueTask<string> ValueTaskFunction2(string name, DateTimeOffset date);
    }
}