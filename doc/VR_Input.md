# VR goggle with PC as an input device

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
