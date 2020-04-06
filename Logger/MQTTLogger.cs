using System;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Logger
{
    public class MQTTLogger
    {
        private MqttClient client;
        private string clientId;

        public MQTTLogger() {
            string BrokerAddress = "io.adafruit.com";

            client = new MqttClient(BrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            //client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            //client.MqttMsgPublished += Client_MqttMsgPublished;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId, "110mat110", "aio_SnuZ72E3ElmV3uA8DP5nw1Qv3ath");
            //Subscribe();
        }

        private void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e) {
            if (e.IsPublished)
                Debug.WriteLine(e.MessageId.ToString());
            else
                Debug.WriteLine("Something goes wrong");
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            // we need this construction because the receiving code in the library and the UI with textbox run on different threads
            Console.WriteLine(ReceivedMessage);

        }

        private void Subscribe() {

            // whole topic
            string Topic = "110mat110/feeds/messages";

            // subscribe to the topic with QoS 2
            client.Subscribe(new string[] { Topic }, new byte[] { 1 });   // we need arrays as parameters because we can subscribe to different topics with one call
        }

        public void Publish(string message) {
            // whole topic
            string Topic = "110mat110/feeds/default.messages";

            // publish a message with QoS 2
            client.Publish(Topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }

        public void Unsubscribe() {
            client.Disconnect();
        }
    }
}
