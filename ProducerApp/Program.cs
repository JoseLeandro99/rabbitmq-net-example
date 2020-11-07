using System;
using System.Text;
using RabbitMQ.Client;

namespace ProducerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = string.Empty;

            while (!message.Equals("0"))
            {
                Console.Write("Type your message: ");
                message = Console.ReadLine();

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
                            arguments : null);

                        var body = Encoding.UTF8.GetBytes(message);

                        chanel.BasicPublish(
                            exchange: "",
                            routingKey: "hello",
                            basicProperties : null,
                            body : body
                        );

                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }
        }
    }
}