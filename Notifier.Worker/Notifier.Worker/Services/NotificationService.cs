using Confluent.Kafka;
using Notifier.Api.Models.Notification;
using System.Net;
using System.Text.Json;

namespace Notifier.Worker.Services
{
    public class NotificationService
    {
        private ConsumerConfig config;
        private ConsumerBuilder<string, string> ConsumerBuilder;
        private IConsumer<string, string> Consumer;

        public NotificationService()
        {
            var kafkaIp = Dns.Resolve(Environment.GetEnvironmentVariable("KAFKA_HOST")).AddressList.Where(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
            this.config = new ConsumerConfig()
            {
                BootstrapServers = $"{kafkaIp}:{Environment.GetEnvironmentVariable("KAFKA_PORT")}",
                GroupId = "notification-group-1"
            };
            this.ConsumerBuilder = new ConsumerBuilder<string, string>(this.config);
            this.Consumer = this.ConsumerBuilder.Build();
        }
        public void Register()
        {
            this.Consumer.Subscribe(Environment.GetEnvironmentVariable("KAFKA_TOPIC"));
            Task.Run(() =>
            {
                while (true)
                {
                    // Execute process
                    Worker();
                    // Prevent other events
                    Thread.Sleep(1);
                }
            });
        }
        private void Worker()
        {
            try
            {
                var message = this.Consumer.Consume();
                var request = JsonSerializer.Deserialize<PushOneRequest>(message.Message.Value);
                if (request != null)
                {
                    Console.WriteLine($"New Message: {request.Phone}: {request.Message}");
                }
                else
                {
                    Console.Error.WriteLine("Unknown request sent!");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}
