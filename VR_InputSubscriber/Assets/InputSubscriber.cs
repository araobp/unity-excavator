using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class InputSubscriber : MonoBehaviour
{
    [SerializeField] Properties properties;
    [SerializeField] Text text;

    IMqttClient mqttClient;

    string m_inputKeycode = "?";
    
    // Start is called before the first frame update
    async void Start()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId("VR")
            .WithTcpServer(properties.mqttServer, 1883)
            .WithCredentials(properties.mqttUsername, Encoding.ASCII.GetBytes(properties.mqttPassword))
            .Build();

        var cancellationTokenSource = new CancellationTokenSource(500);

        try
        {
            await mqttClient.ConnectAsync(options, cancellationTokenSource.Token);
            if (mqttClient.IsConnected)
            {
                Debug.Log("Connected to MQTT server");
                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    var message = e.ApplicationMessage;
                    Debug.Log(e);
                    if (message.Topic == properties.topic)
                    {
                        string payload = Encoding.UTF8.GetString(message.Payload);
                        Debug.Log($"MQTT message received: {payload}");
                        text.text = payload;
                    }
                });

                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    var message = e.ApplicationMessage;
                    Debug.Log(e);
                    if (message.Topic == properties.topic)
                    {
                        string payload = Encoding.UTF8.GetString(message.Payload);
                        m_inputKeycode = payload;
                    }
                });

                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(properties.topic).Build());
            }
            else
            {
                Debug.Log("MQTT connection failure");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log($"MQTT connection failure: {e}");
        }
    }

    private void Update()
    {
        text.text = m_inputKeycode;
    }
}
