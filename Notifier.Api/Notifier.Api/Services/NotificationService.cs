using Confluent.Kafka;
using Notifier.Api.Models.Notification;
using System.Net;
using System.Text.Json;

namespace Notifier.Api.Services
{
    public class NotificationService
    {
        private ProducerConfig config;
        private ProducerBuilder<string, string> producerBuilder;
        private IProducer<string, string> producer;

        public NotificationService()
        {
            var kafkaIp = Dns.Resolve(Environment.GetEnvironmentVariable("KAFKA_HOST")).AddressList.Where(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
            this.config = new ProducerConfig()
            {
                BootstrapServers = $"{kafkaIp}:{Environment.GetEnvironmentVariable("KAFKA_PORT")}"
            };
            this.producerBuilder = new ProducerBuilder<string, string>(this.config);
            this.producer = this.producerBuilder.Build();
        }
        public void Push(PushOneRequest request)
        {
            this.producer.Produce(Environment.GetEnvironmentVariable("KAFKA_TOPIC"), new Message<string, string>()
            {
                Key = "notification",
                Value = JsonSerializer.Serialize(request, request.GetType())
            });
        }
    }
}
