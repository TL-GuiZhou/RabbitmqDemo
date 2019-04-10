using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Net.Rabbitmq.Receive
{
    class Program
    {
        private static string MqKey;
        static void Main(string[] args)
        {
            Console.WriteLine("plese enter you queuename:");
            MqKey = Console.ReadLine();
            var connectionFactory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "root",
                Password = "root"
            };
            //2.建立连接
            using (var connection = connectionFactory.CreateConnection())
            {
                //3. 创建信道
                using (var model = connection.CreateModel())
                {
                    //申明队列
                    //durable: 消息持久化 
                    model.ExchangeDeclare(exchange: "exchangename", type: "fanout");
                    model.QueueDeclare(queue: MqKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    model.QueueBind(queue: MqKey, exchange: "exchangename", routingKey: "");
                    Receive(model);
                    Console.ReadLine();
                }
            }
          
        }

        public static void Receive(IModel model)
        {
            //构建消费者
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, ea) =>
            {
                Console.WriteLine($" [x] Received {Encoding.UTF8.GetString(ea.Body)}");
                model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            //启动消费者实例
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
            model.BasicConsume(queue: MqKey, autoAck: false, consumer: consumer);
            Console.WriteLine("Start Receive data");
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
           
        }


     
    }
}
