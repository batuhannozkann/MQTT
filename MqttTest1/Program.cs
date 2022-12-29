using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Text;

namespace MqttTest1
{
    
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.mqttdashboard.com", 1883)
                .WithCleanSession()
                .Build();
            var topicfilter = new TopicFilterBuilder().WithTopic("testtopic/11").Build();
            await mqttClient.ConnectAsync(options);
            await mqttClient.SubscribeAsync(topicfilter);
            
            mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                  var msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine(msg);
                var applicationMessage = new MqttApplicationMessageBuilder().WithTopic("testtopic/12").WithPayload(msg)
                .WithExactlyOnceQoS().WithRetainFlag().Build();
                await mqttClient.PublishAsync(applicationMessage);
            });
            
            Console.ReadLine();





        }
    }
}