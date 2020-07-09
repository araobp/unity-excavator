# Integration with TensorFlow Lite on Android

![TensorFlowLite](./TensorFlowLite.png)

## Architecture

```
Ethan and the car               MobileNetv2/TensorFlowLite
are on Unity                    runs on Android for object detection
 [Unity] <----> [mosquitto] <----> [Android]
                MQTT server
                username: simulator
                password: simulator
```

## Code
- [=> Code on Unity](../TensorFlowLite)
- [=> Code on Android](../android/camera)
