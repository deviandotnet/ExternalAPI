using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ExternalAPI.Helpers
{
    public class RabbitPublisher
    {
        private readonly ConnectionFactory _connectionFactory;



        public RabbitPublisher(IConfiguration configuration)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost", //give a proper hostname or IP address of the RabbitMQ server
                Port = 15672, //default RabbitMQ port
            };

      
        }

        public async Task PublishAsync<T>(T message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "record", durable: true, exclusive: false, autoDelete: false, arguments: null);

            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: "record.created",
                    mandatory: true,
                    basicProperties: new BasicProperties { Persistent = true },
                    body: body);

            }
            catch (Exception)
            {

                throw;
            }
            
        }


    }
}
