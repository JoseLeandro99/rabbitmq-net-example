using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            {
                using(var chanel = connection.CreateModel())
                {
                    chanel.QueueDeclare(
                        queue: "hello",
                        durable : false,
                        exclusive : false,
                        autoDelete : false,
                        arguments : null
                    );

                    var consumer = new EventingBasicConsumer(chanel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    chanel.BasicConsume(
                        queue: "hello",
                        autoAck : true,
                        consumer : consumer
                    );

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}