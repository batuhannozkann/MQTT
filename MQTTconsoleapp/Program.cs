using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json.Linq;
using System.Text;


namespace MQTTconsoleapp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("cloud.thingson.io", 1883)
                .WithKeepAliveSendInterval(TimeSpan.FromSeconds(300)).WithClientId("Client1")
                .WithCredentials(username:"YDTM2ySkNT0ibg17rplY", password: "")
                .WithCleanSession(false).Build();
            await mqttClient.ConnectAsync(options);
            Console.WriteLine("Connection: "+mqttClient.IsConnected);
            string requesttopic = $"v1/devices/me/rpc/request/+";
            var subs=await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(requesttopic).Build());
            Console.WriteLine(subs.Items[0].ResultCode);
            Console.WriteLine(subs.Items[0].TopicFilter);
            mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {

                var msg = JObject.Parse(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                string mesaj = msg["params"].ToString();
                Console.WriteLine(msg);
                Console.WriteLine(mesaj);
                string requesttopicnow = e.ApplicationMessage.Topic;
                dynamic value=(requesttopicnow.Split("/").Last());
                await mqttClient.PublishAsync($"v1/devices/me/rpc/response/{value}",mesaj);
            });
            Console.ReadLine();
            
        }
    }
}