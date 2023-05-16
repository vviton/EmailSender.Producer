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
        private readonly ConnectionFactory _factory;

        public EmailProducer(string hostName, int portNumber)
        {
            _factory = new ConnectionFactory() { HostName = hostName, Port = portNumber };
        }

        public void SendToQueue(Email message)
        {
         
            using (var connection = _factory.CreateConnection())
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
