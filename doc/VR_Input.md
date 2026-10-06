# VR goggle with PC as an input device

I've got a cheap VR goggle, but it has no input devices.

As usual, I use MQTT in such a case to transfer user input events to a VR app on Android.

## PC side (MQTT client)
<img src="/doc/InputPublisher.jpg" width=200px>

## Android side (MQTT client)
<img src="/doc/InputSubscriber.png" width=600px>

## Architecture

```
   [PC]----- MQTT --->[MQTT server]---- MQTT --->[Android]
```

## Code

[=> PC side](/VR_InputPublisher)

[=> Android side](/VR_InputSubscriber)
