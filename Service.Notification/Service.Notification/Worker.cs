using Service.Notification.Services.Contracts;

namespace Service.Notification
{
    public class Worker : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;

        public Worker(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Iniciar o serviço do RabbitMQ
                await _rabbitMqService.ProcessQueue(stoppingToken);

                // Tempo de espera entre as iterações
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
