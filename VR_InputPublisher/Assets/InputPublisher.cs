using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class InputPublisher : MonoBehaviour
{
    [SerializeField] Properties properties;
    [SerializeField] Text text;

    IMqttClient mqttClient;

    private void Publish(string message)
    {
        text.text = message;

        byte[] bytes = Encoding.ASCII.GetBytes(message);

        if (mqttClient.IsConnected)
        {
            var payload = new MqttApplicationMessageBuilder()
            .WithTopic(properties.topic)
            .WithPayload(bytes)
            .WithRetainFlag(false)
            .Build();
            mqttClient.PublishAsync(payload);
        }

        
    }

    // Start is called before the first frame update
    async void Start()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId("input")
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
        if (Input.GetKey(KeyCode.W))
        {
            Publish("W");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Publish("A");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Publish("S");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Publish("D");
        }
        else if (Input.GetKey(KeyCode.K))
        {
            Publish("K");
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            Publish("1");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Publish("2");
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            Publish("3");
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            Publish("4");
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            Publish("5");
        }
    }

}
