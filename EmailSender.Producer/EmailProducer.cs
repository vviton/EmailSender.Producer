using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Producer
{
    public class EmailProducer : IRabbitMqProducer<Email>
    {
        private readonly string _rabbitConnection;

        public EmailProducer()
        {
            _rabbitConnection = "localhost";
        }

        public EmailProducer(string rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public void SendToQueue(Email message)
        {
            var factory = new ConnectionFactory { HostName = _rabbitConnection };
            using (var connection = factory.CreateConnection())
            {
                using (var emailChannel = connection.CreateModel())
                {
                    emailChannel.QueueDeclare(
                        queue: "email_message",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var jsonEmail = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonEmail);
                    emailChannel.BasicPublish(exchange:"",routingKey:"email_message",body:body);
                }
            }
        }
    }
}
