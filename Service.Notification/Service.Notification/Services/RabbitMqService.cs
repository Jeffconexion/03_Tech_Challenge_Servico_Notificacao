using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Notification.Domain;
using Service.Notification.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace Service.Notification.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private const string QUEUE_NOTIFICATION = "fila-notificacoes";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEmailService _emailService;

        public RabbitMqService(IEmailService emailService)
        {
            _emailService = emailService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QUEUE_NOTIFICATION,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task ProcessQueue(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var paymentApprovedBytes = eventArgs.Body.ToArray();
                var paymentApprovedJson = Encoding.UTF8.GetString(paymentApprovedBytes);

                var messageNotification = JsonSerializer.Deserialize<MessageNotification>(paymentApprovedJson);

                //chamo o envio de email
                await _emailService.SendEmailAsync(messageNotification.Name, messageNotification.Email, messageNotification.Message);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE_NOTIFICATION, false, consumer);
        }
    }
}
