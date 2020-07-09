package araobp.camera.net

import org.eclipse.paho.client.mqttv3.MqttMessage

interface IMqttReceiver {
    fun messageArrived(topic: String?, message: MqttMessage?)
}