using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Net.Rabbitmq.Publish
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
            model.QueueDeclare();
            model.ExchangeDeclare(exchange: "exchangename", type: "fanout");
            while (true)
            {
                var info=Console.ReadLine();
                var message = $"{DateTime.Now:HH:mm:ss tt zz} with {info}";
                model.BasicPublish("exchangename", "", null, Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Publish: {message}");
            }
        }
    }
}
