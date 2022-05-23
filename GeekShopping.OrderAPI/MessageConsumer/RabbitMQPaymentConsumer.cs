using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private const string _exchange = "DirectPaymentUpdateExchange";
        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQPaymentConsumer(OrderRepository repository)
        {
            _repository = repository;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = AmqpTcpEndpoint.UseDefaultPort
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchange, ExchangeType.Direct);
            _channel.QueueDeclare(_paymentOrderUpdateQueueName, false, false, false, null);
            _channel.QueueBind(_paymentOrderUpdateQueueName, _exchange, "PaymentOrder");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultMessage message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
                UpdatePaymentStatus(message).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_paymentOrderUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultMessage message)
        {
            try
            {
                await _repository.UpdateOrderPaymentStatus(message.OrderId, message.Status);
            }
            catch (Exception)
            {
                // Log
                throw;
            }
        }
    }
}
