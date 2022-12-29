using Newtonsoft.Json.Linq;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2qtt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MqttClient mqttClient = new MqttClient("cloud.thingson.io", 1883, false, null, null, MqttSslProtocols.None);
            mqttClient.Connect("Client1", "YDTM2ySkNT0ibg17rplY", "");
            dynamic value = new JObject();
            value.variable = 3;
            string requesttopic = $"v1/devices/me/rpc/request/{value.variable}";
            mqttClient.MqttMsgSubscribed += mqttClient_MqttMsgSubscribed;
            var subs = mqttClient.Subscribe(new string[] { requesttopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            Console.WriteLine(subs);
            mqttClient.MqttMsgPublishReceived += mqttClient_MqttMsgPublishReceived;
            static void mqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
            {
                var msg = JObject.Parse(Encoding.UTF8.GetString(e.Message));
                string mesaj = msg["params"].ToString();
                Console.WriteLine(mesaj);
            }
            static void mqttClient_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
            {
                Console.WriteLine(e.MessageId);
            }
            Console.WriteLine(mqttClient.IsConnected);
            mqttClient.Disconnect();
            Console.ReadLine();
        }
    }
}