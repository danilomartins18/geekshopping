using GeekShopping.PaymentAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentAPI.MessageSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        private const string _exchange = "DirectPaymentUpdateExchange";
        private const string _paymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(_exchange, ExchangeType.Direct, false);

                channel.QueueDeclare(_paymentEmailUpdateQueueName, false, false, false, null);
                channel.QueueDeclare(_paymentOrderUpdateQueueName, false, false, false, null);

                channel.QueueBind(_paymentEmailUpdateQueueName, _exchange, "PaymentEmail");
                channel.QueueBind(_paymentOrderUpdateQueueName, _exchange, "PaymentOrder");

                byte[] body = GetMessageAsByteArray(message);
                channel.BasicPublish(exchange: _exchange, routingKey: "PaymentEmail", basicProperties: null, body: body);
                channel.BasicPublish(exchange: _exchange, routingKey: "PaymentOrder", basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize((UpdatePaymentResultMessage)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password,
                    Port = AmqpTcpEndpoint.UseDefaultPort
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;
            CreateConnection();
            return _connection != null;
        }
    }
}
