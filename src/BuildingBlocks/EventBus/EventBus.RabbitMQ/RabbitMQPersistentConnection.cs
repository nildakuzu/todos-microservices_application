﻿using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private IConnection connection;
        private readonly IConnectionFactory connectionFactory;
        private readonly int retryCount;
        private object lock_object;
        private bool _dispose;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 3)

        {
            this.connectionFactory = connectionFactory;
            this.retryCount = retryCount;
            lock_object = new object();
        }

        public bool IsConnected => connection != null && connection.IsOpen;

        public void Dispose()
        {
            _dispose = true;
            connection?.Dispose();
        }

        public IModel CreateModel()
        {
            return connection.CreateModel();
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>().Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryCount)), (ex, time) =>
                       {

                       });

                policy.Execute(() =>
                {
                    connection = connectionFactory.CreateConnection();

                });
            }

            if (IsConnected)
            {
                connection.ConnectionShutdown += Connection_ConnectionShutdown;
                connection.CallbackException += Connection_CallbackException;
                connection.ConnectionBlocked += Connection_ConnectionBlocked;
                return true;
            }

            return false;
        }

        private void Connection_ConnectionBlocked(object sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_dispose)
                return;

            TryConnect();
        }

        private void Connection_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_dispose)
                return;

            TryConnect();
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_dispose)
                return;

            TryConnect();
        }
    }
}
