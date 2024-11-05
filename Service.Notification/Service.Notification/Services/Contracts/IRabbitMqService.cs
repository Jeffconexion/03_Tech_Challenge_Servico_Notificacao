namespace Service.Notification.Services.Contracts
{
    public interface IRabbitMqService
    {
        Task ProcessQueue(CancellationToken cancellationToken);
    }
}
