using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Notification.Domain;
using Service.Notification.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace Service.Notification.Services
{
    public class MessageBusService : IMessageBusService
    {
        private const string QUEUE_NOTIFICATION = "fila-notificacoes";
        private readonly IEmailService _emailService;
        private readonly ConnectionFactory _factory;

        public MessageBusService(IEmailService emailService)
        {
            _emailService = emailService;
            _factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
        }

        public async Task ProcessQueue(CancellationToken cancellationToken)
        {
            using (var connection = _factory.CreateConnection())
            {
                var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);

                channel.QueueDeclare(
                                    queue: QUEUE_NOTIFICATION,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null
                                    );

                consumer.Received += async (sender, eventArgs) =>
                {
                    try
                    {
                        var paymentApprovedBytes = eventArgs.Body.ToArray();
                        var paymentApprovedJson = Encoding.UTF8.GetString(paymentApprovedBytes);

                        var messageNotification = JsonSerializer.Deserialize<MessageNotification>(paymentApprovedJson);

                        Console.WriteLine(messageNotification.Name + " " + messageNotification.Email + " " + messageNotification.Message);
                        await _emailService.SendEmailAsync(messageNotification.Name, messageNotification.Email, messageNotification.Message);

                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                        channel.BasicNack(eventArgs.DeliveryTag, false, true);
                    }
                };

                channel.BasicConsume(QUEUE_NOTIFICATION, false, consumer);

                try
                {
                    await Task.Delay(-1, cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine("Consumer stopping gracefully.");
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    channel.Close();
                }
                finally
                {
                    channel.Close();
                }
            }
        }
    }
}
