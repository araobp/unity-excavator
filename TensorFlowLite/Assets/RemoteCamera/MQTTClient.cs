using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

// MQTT-related properties
public static class Properties
{
    public static string mqttServer = "192.168.57.3";
    public static string mqttUsername = "simulator";
    public static string mqttPassword = "simulator";
    public static string topic = "camera";
    public static string topicDepth = "cameraDepth";
}

// Schema for JSON messaging over MQTT
public class RequestedCommand
{
    public string command;
    public float[] values;
}

public class MQTTClient : MonoBehaviour
{

    Camera remoteCamera;
    Camera remoteCameraDepth;
    Transform remoteCameraTransform;
    Transform remoteCameraDepthTransform;

    IMqttClient mqttClient;

    bool captureRequested = false;
    float deltaPan = 0F;
    float deltaTilt = 0F;
    float deltaZoom = 0F;

    // Capture an image from a camera
    private void Capture(Camera camera, string topic)
    {
        // Note: only a main thread is allowd to perform the following operation.
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        camera.Render();

        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToJPG();
        Destroy(image);

        if (mqttClient.IsConnected)
        {
            var message = new MqttApplicationMessageBuilder()
            .WithTopic($"{topic}Tx")
            .WithPayload(bytes)
            .WithRetainFlag(false)
            .Build();
            mqttClient.PublishAsync(message);
        }
    }

    // Start is called before the first frame update
    async void Start()
    {
        GameObject remoteCameraObject = GameObject.Find("RemoteCamera");
        GameObject remoteCameraDepthObject = GameObject.Find("RemoteCameraDepth");

        remoteCamera = remoteCameraObject.GetComponent<Camera>();
        remoteCameraDepth = remoteCameraDepthObject.GetComponent<Camera>();
        remoteCameraDepth.depthTextureMode |= DepthTextureMode.Depth;

        remoteCameraTransform = remoteCameraObject.transform;
        remoteCameraDepthTransform = remoteCameraDepthObject.transform;

        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId("simulator")
            .WithTcpServer(Properties.mqttServer, 1883)
            .WithCredentials(Properties.mqttUsername, Encoding.ASCII.GetBytes(Properties.mqttPassword))
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
                    if (message.Topic == $"{Properties.topic}Rx")
                    {
                        string payload = Encoding.UTF8.GetString(message.Payload);
                        var requestedCommand = JsonConvert.DeserializeObject<RequestedCommand>(payload);
                        var values = requestedCommand.values;
                        var value = values[0];
                        Debug.Log($"command: {requestedCommand.command}, value: {value}");
                        switch (requestedCommand.command)
                        {
                            case "capture":
                                captureRequested = true;
                                break;
                            case "pan":
                                deltaPan = value;
                                break;
                            case "tilt":
                                deltaTilt = value;
                                break;
                            case "zoom":
                                deltaZoom = value;
                                break;
                        }
                    }
                });

                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic($"{Properties.topic}Rx").Build());

            }
            else
            {
                Debug.Log("MQTT connection failure");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("MQTT connection failure");
        }

    }

    private void FixedUpdate()
    {
        if (captureRequested)
        {
            Capture(remoteCameraDepth, Properties.topicDepth);
            Capture(remoteCamera, Properties.topic);
            captureRequested = false;
        }

        if (deltaPan != 0F)
        {
            remoteCameraTransform.Rotate(0, deltaPan, 0);
            remoteCameraDepthTransform.Rotate(0, deltaPan, 0);
            deltaPan = 0F;
        }

        if (deltaTilt != 0F)
        {
            remoteCameraTransform.Rotate(deltaTilt, 0, 0);
            remoteCameraDepthTransform.Rotate(deltaTilt, 0, 0);
            deltaTilt = 0F;
        }

        if (deltaZoom != 0F)
        {
            remoteCamera.fieldOfView += deltaZoom;
            remoteCameraDepth.fieldOfView += deltaZoom;
            deltaZoom = 0F;
        }

    }

}
