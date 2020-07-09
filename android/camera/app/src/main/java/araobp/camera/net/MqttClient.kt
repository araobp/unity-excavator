package araobp.camera.net

import android.content.Context
import android.util.Log
import org.eclipse.paho.android.service.MqttAndroidClient
import org.eclipse.paho.client.mqttv3.*
import org.json.JSONArray
import org.json.JSONObject

class MqttClient(
    val context: Context,
    val mqttServer: String,
    val mqttUsername: String,
    val mqttPassword: String,
    val clientId: String,
    val receiver: IMqttReceiver
) : MqttCallback {

    private var mTopicList = ArrayList<String>()

    override fun messageArrived(topic: String?, message: MqttMessage?) {
        receiver.messageArrived(topic, message)
    }

    override fun connectionLost(cause: Throwable?) {
        //TODO("Not yet implemented")
    }

    override fun deliveryComplete(token: IMqttDeliveryToken?) {
        //TODO("Not yet implemented")
    }

    companion object {
        val TAG: String = this::class.java.simpleName
        val MQTT_PORT = "1883"
    }

    private val mMqttClient = MqttAndroidClient(
        context, "tcp://$mqttServer:$MQTT_PORT", clientId
    )

    fun publish(topic: String, jsonObject: JSONObject) {
        if (mMqttClient.isConnected) {
            val mqttMessage =
                MqttMessage(jsonObject.toString().toByteArray(charset = Charsets.UTF_8))
            mMqttClient.publish("${topic}Rx", mqttMessage)
        }
    }

    fun subscribe(topic: String) {
        if (mMqttClient.isConnected) {
            mMqttClient.subscribe(topic, 0)
            mTopicList.add(topic)
        }
    }

    fun subscribe(topicList: List<String>) {
        if (mMqttClient.isConnected) {
            topicList.forEach {
                mMqttClient.subscribe(it, 0)
                mTopicList.add(it)
            }
        }
    }

    fun unsubscribeAllAndSubscribe(topic: String) {
        if (mMqttClient.isConnected && mTopicList.size > 0) {
            mTopicList.forEach {
                mMqttClient.unsubscribe("${it}Tx")
            }
            mMqttClient.subscribe("${topic}Tx", 0)
            mTopicList.add(topic)
        }
    }

    fun unsubscribeAllAndSubscribe(topicList: List<String>) {
        if (mMqttClient.isConnected && mTopicList.size > 0) {
            mTopicList.forEach {
                mMqttClient.unsubscribe("${it}Tx")
            }
            topicList.forEach{
                mMqttClient.subscribe("${it}Tx", 0)
                mTopicList.add(it)
            }
        }
    }

    fun connect(topic: String) {
        try {
            mMqttClient.setCallback(this)
            val options = MqttConnectOptions()
            options.userName = mqttUsername
            options.password = mqttPassword.toCharArray()
            mMqttClient.connect(options, null, object : IMqttActionListener {
                override fun onSuccess(iMqttToken: IMqttToken) {
                    try {
                        Log.d(TAG, "onSuccess")
                        mMqttClient.subscribe("${topic}Tx", 0)
                        mTopicList.add(topic)
                    } catch (e: MqttException) {
                        Log.d(TAG, e.toString())
                    }
                }

                override fun onFailure(
                    iMqttToken: IMqttToken,
                    throwable: Throwable
                ) {
                    Log.d(TAG, "onFailure")
                }
            })
        } catch (e: MqttException) {
            Log.d(TAG, e.toString())
        }
    }

    fun connect(topicList: List<String>) {
        try {
            mMqttClient.setCallback(this)
            val options = MqttConnectOptions()
            options.userName = mqttUsername
            options.password = mqttPassword.toCharArray()
            mMqttClient.connect(options, null, object : IMqttActionListener {
                override fun onSuccess(iMqttToken: IMqttToken) {
                    Log.d(TAG, "onSuccess")
                    try {
                        topicList.forEach {
                            mMqttClient.subscribe("${it}Tx", 0)
                            mTopicList.add(it)
                        }
                    } catch (e: MqttException) {
                        Log.d(TAG, e.toString())
                    }
                }

                override fun onFailure(
                    iMqttToken: IMqttToken,
                    throwable: Throwable
                ) {
                    Log.d(TAG, "onFailure")
                }
            })
        } catch (e: MqttException) {
            Log.d(TAG, e.toString())
        }
    }

    fun destroy() {
        mTopicList.forEach {
            mMqttClient.unsubscribe("${it}Tx")
        }
        mMqttClient.unregisterResources()
        mMqttClient.disconnect()
    }

}