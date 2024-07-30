namespace Mangoes.Services.EmailAPI.Messaging
{
    public interface IAzureBusServiceConsumer
    {
        Task Stop();
        Task Start();
    }
}
