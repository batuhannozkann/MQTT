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
            byte[] payload = Encoding.UTF8.GetBytes("Yeni Mesaj");
            dynamic variable = new JObject();
            variable.value = 4;
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var responsetopic = $"v1/devices/me/rpc/response/{variable.value}";
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("cloud.thingson.io", 1883)
                .WithKeepAliveSendInterval(TimeSpan.FromSeconds(300)).WithClientId("Client1")
                .WithCredentials(username:"YDTM2ySkNT0ibg17rplY", password: "")
                .WithCleanSession(false).Build();
            await mqttClient.ConnectAsync(options);
            Console.WriteLine("Connection: "+mqttClient.IsConnected);


            var requesttopic = $"v1/devices/me/rpc/request/{variable.value}";

            var subs=await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(responsetopic).WithAtMostOnceQoS().Build());
            Console.WriteLine(subs.Items[0].ResultCode);
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            });
            
              //  var msg = JObject.Parse(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                //string message = msg["params"].ToString();
                //Console.WriteLine(message);
            
            
            Console.ReadLine();
            Console.WriteLine(mqttClient.IsConnected);
            
            
            
            
            


        }
    }
}