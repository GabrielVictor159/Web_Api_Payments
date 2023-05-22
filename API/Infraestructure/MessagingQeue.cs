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
        private readonly string _rabbitMqConnectionString = "";
        private readonly int _timeRequest = 10;
        public delegate Task HandleMessage(string e, IModel? channel, Object? model, BasicDeliverEventArgs? args);
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

        public void SendDirectExchange(string exchangeName, string routingKey, string message, Boolean persistent = false)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return;
            }
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = persistent;
                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: properties, body: body);
                }
            }
        }

        public void SendTopicExchange(string exchangeName, string routingKey, string message, Boolean persistent = false)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return;
            }
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = persistent;
                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: properties, body: body);
                }
            }
        }

        public void SendFanoutExchange(string exchangeName, string message, Boolean persistent = false)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return;
            }
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = persistent;
                    channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: properties, body: body);
                }
            }
        }

        public void SendDefaultQueue(string queueName, string message, Boolean persistent = false)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return;
            }
            using (var connection = CreateConnection())
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

        public async Task<string?> ReceiveDirectExchange(string exchangeName, string queueName, string routingKey, HandleMessage handleMessage)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return null;
            }
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.ToArray());
                        await handleMessage.Invoke(message, channel, model, args);
                        cancellationTokenSource.Cancel();
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    await Task.Delay(TimeSpan.FromSeconds(_timeRequest), cancellationToken);
                    return null;
                }
            }
        }


        public async Task<string?> ReceiveTopicExchange(string exchangeName, string queueName, string routingKeyPattern, HandleMessage handleMessage)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return null;
            }
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKeyPattern);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.ToArray());
                        await handleMessage.Invoke(message, channel, model, args);
                        cancellationTokenSource.Cancel();
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    await Task.Delay(TimeSpan.FromSeconds(_timeRequest), cancellationToken);
                    return null;
                }
            }
        }

        public async Task<string?> ReceiveFanoutExchange(string exchangeName, string queueName, HandleMessage handleMessage)
        {
            if (string.IsNullOrEmpty(_rabbitMqConnectionString))
            {
                return null;
            }
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.ToArray());
                        await handleMessage.Invoke(message, channel, model, args);
                        cancellationTokenSource.Cancel();
                    };
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    await Task.Delay(TimeSpan.FromSeconds(_timeRequest), cancellationToken);
                    return null;
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
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.ToArray());
                        await handleMessage.Invoke(message, channel, model, args);
                        cancellationTokenSource.Cancel();
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    await Task.Delay(TimeSpan.FromSeconds(_timeRequest), cancellationToken);
                    return null;
                }
            }
        }

        private IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqConnectionString)
            };

            return factory.CreateConnection();
        }
    }
}
