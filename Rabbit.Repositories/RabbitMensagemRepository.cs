using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Rabbit.Repositories
{
    public class RabbitMensagemRepository : IRabbitMensagemRepository
    {
        public void SendMensagem(RabbitMensagem mensagem)
        {
            var factory = new ConnectionFactory 
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "rabbitMensagensQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string json = JsonSerializer.Serialize(mensagem);

            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "rabbitMensagensQueue",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
