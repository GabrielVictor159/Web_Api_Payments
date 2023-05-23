using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace API.Infraestructure
{

    public class MessagingQeue : IMessagingQeue
    {
        private readonly string _rabbitMqConnectionString = "rabbitmq";
        private readonly int _timeRequest = 10;
        public delegate Task HandleMessage(string e, IModel? channel, Object? model, BasicDeliverEventArgs? args);
        public delegate Task ConsumerConfig(IModel? channel);
        public MessagingQeue(IConfiguration configuration)
        {
            var stringConection = configuration.GetValue<string>("MessagingQeueConnectionString");
            var timeRequest = configuration.GetValue<int?>("MessagingQeueTimeSecondsRequest");
            if (stringConection != null)
            {
                _rabbitMqConnectionString = stringConection;
            }
            if (timeRequest != null)
            {
                _timeRequest = (int)timeRequest;
            }
        }


        public void SendDefaultQueue(string queueName, string message, Boolean persistent = false)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return;
            }
            using (var connection = CreateConnection(queueName))
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = persistent;
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
                }
            }
        }


        public async Task<string?> ReceiveDefaultQueue(string queueName, HandleMessage handleMessage)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return null;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            using (var connection = CreateConnection(queueName))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, args) =>
                {
                    var message = Encoding.UTF8.GetString(args.Body.ToArray());
                    await handleMessage.Invoke(message, channel, model, args);
                };
                string consumerTag = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                channel.BasicCancel(consumerTag);
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_timeRequest), cancellationToken);
                }
                catch (OperationCanceledException)
                {

                }

                return null;
            }
        }





        private IConnection CreateConnection(string? name = "")
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConnectionString
            };

            return factory.CreateConnection(name);
        }
    }
}
