﻿using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetCore.Rabbitmq.ConsoleApp
{
    class Program
    {
        private static string MqKey => "mqkeyname";
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "root",
                Password = "root"
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Publish(channel);
                }
            }
            Console.ReadLine();
        }

        public static void Publish(IModel model)
        {
            model.QueueDeclare(MqKey, false, false, false, null);
            while (true)
            {
                var message = $"Hello World{DateTime.Now:HH:mm:ss tt zz}";
                model.BasicPublish("", MqKey, null, Encoding.UTF8.GetBytes(message));
                Console.WriteLine($" set {message}");
                Console.ReadLine();
            }
        }
     

    }
}
