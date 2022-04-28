using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {

        RabbitMQPersistentConnection persistentConnection;
        private readonly IConnectionFactory connectionFactory;
        private readonly IModel consumerChannel;

        public EventBusRabbitMQ(IServiceProvider serviceProvider, EventBusConfig eventBusConfig) : base(serviceProvider, eventBusConfig)
        {


            if (string.IsNullOrEmpty(eventBusConfig.HostName) == false)
            {
                connectionFactory = new ConnectionFactory()
                {
                    HostName = eventBusConfig.HostName,
                    Port = 5672
                };
            }
            else
            {
                connectionFactory = new ConnectionFactory();
            }

            persistentConnection = new RabbitMQPersistentConnection(connectionFactory, eventBusConfig.ConnectionRetryCount);
            consumerChannel = CreateConsumerChannel();
            EventBusSubscriptionManager.OnEventRemoved += EventBusSubscriptionManager_OnEventRemoved;
        }

        private void EventBusSubscriptionManager_OnEventRemoved(object sender, string eventName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            consumerChannel.QueueUnbind(queue: eventName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);

            if (EventBusSubscriptionManager.IsEmpty)
            {
                consumerChannel.Close();
            }
        }

        public override void Publish(IntegrationEvent eventMessage)
        {
            if (persistentConnection.IsConnected == false)
            {
                persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(EventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {

                });


            policy.Execute(() =>
            {
                var eventName = eventMessage.GetType().Name;

                consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

                var message = JsonConvert.SerializeObject(eventMessage);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                consumerChannel.QueueDeclare(queue: eventName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                consumerChannel.QueueBind(queue: eventName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName, null);

                consumerChannel.BasicPublish(exchange: EventBusConfig.DefaultTopicName, routingKey: eventName, mandatory: true, basicProperties: properties, body: body);
            });

        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;

            if (EventBusSubscriptionManager.HasSubscriptionForEvent(eventName) == false)
            {
                if (persistentConnection.IsConnected == false)
                {
                    persistentConnection.TryConnect();
                }
                consumerChannel.QueueDeclare(queue: eventName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                consumerChannel.QueueBind(queue: eventName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);
            }

            EventBusSubscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubsribe<T, TH>()
        {
            EventBusSubscriptionManager.RemoveSubscription<T, TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(consumerChannel);

                consumer.Received += Consumer_Received;

                consumerChannel.BasicConsume(queue: eventName, autoAck: false, consumerTag: EventBusConfig.SubscriberClientName, consumer: consumer);
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;

            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception)
            {

                throw;
            }

            consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        }
    }
}
