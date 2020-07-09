package araobp.camera

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.os.Bundle
import android.util.Log
import android.view.MotionEvent
import androidx.appcompat.app.AppCompatActivity
import araobp.camera.aicamera.ObjectDetector
import araobp.camera.net.IMqttReceiver
import araobp.camera.net.MqttClient
import kotlinx.android.synthetic.main.ptz_camera.*
import org.eclipse.paho.client.mqttv3.MqttMessage
import org.json.JSONArray
import org.json.JSONObject
import java.util.*
import kotlin.concurrent.scheduleAtFixedRate
import kotlin.properties.Delegates


class PtzCameraActivity : AppCompatActivity() {

    private lateinit var mMqttClient: MqttClient
    private lateinit var mqttServer: String
    private lateinit var mqttUsername: String
    private lateinit var mqttPassword: String
    private var depthCameraRange by Delegates.notNull<Float>()

    private lateinit var objectDetector: ObjectDetector

    private var requestedCommand: JSONObject? = null
    private var imageDepth: Bitmap? = null

    enum class Command {
        pan,
        tilt,
        zoom,
        capture
    }

    companion object {
        val TAG: String = this::class.java.simpleName
        val TOPIC = "camera"
        val TOPIC_DEPTH = "cameraDepth"
    }

    val mqttReceiver = object : IMqttReceiver {
        override fun messageArrived(topic: String?, message: MqttMessage?) {
            message?.let {
                Log.d(TAG, "mqtt message received on ${topic}")
                val jpegByteArray = it.payload
                val bitmap = BitmapFactory.decodeByteArray(jpegByteArray, 0, jpegByteArray.size)
                if (topic == "${TOPIC}Tx") {
                    val newBitmap = objectDetector.detect(bitmap, imageDepth)
                    imageViewPtzCamera.setImageBitmap(newBitmap)
                } else if (topic == "${TOPIC_DEPTH}Tx") {
                    imageViewPtzCameraDepth.setImageBitmap(bitmap)
                    imageDepth = bitmap
                }
            }
        }
    }

    private fun command(command: Command, value: Float): JSONObject {
        val values = JSONArray()
        values.put(value)
        return JSONObject()
            .put("command", command.toString())
            .put("values", values)
    }

    private fun command(command: Command): JSONObject {
        val values = JSONArray()
        values.put(0F);
        return JSONObject()
            .put("command", command.toString())
            .put("values", values)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.ptz_camera)

        supportActionBar!!.setDisplayHomeAsUpEnabled(true)

        mqttServer = intent.getStringExtra(MainActivity.MQTT_SERVER)
        mqttUsername = intent.getStringExtra(MainActivity.MQTT_USERNAME)
        mqttPassword = intent.getStringExtra(MainActivity.MQTT_PASSWORD)
        depthCameraRange = intent.getFloatExtra(MainActivity.DEPTH_CAMERA_RANGE, 50F)
        objectDetector = ObjectDetector(this, depthCameraRange)

        buttonTiltUp.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.tilt, 1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        buttonTiltDown.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.tilt, -1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        buttonPanRight.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.pan, -1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        buttonPanLeft.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.pan, 1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        buttonZoomIn.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.zoom, -1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        buttonZoomOut.setOnTouchListener { _, event ->
            if (event.action == MotionEvent.ACTION_DOWN) {
                requestedCommand = command(Command.zoom, 1F)
            } else if (event.action == MotionEvent.ACTION_UP) {
                requestedCommand = null
            }
            true
        }

        Timer().scheduleAtFixedRate(250, 250) {  // throttling at a certain interval
            requestedCommand?.let { mMqttClient.publish(TOPIC, it) }
        }

        Timer().scheduleAtFixedRate(1000, 1000) {  // capturing image periodically
            mMqttClient.publish(TOPIC, command(Command.capture))
        }

    }

    override fun onResume() {
        super.onResume()
        mMqttClient = MqttClient(
            context = this,
            mqttServer = mqttServer,
            mqttUsername = mqttUsername,
            mqttPassword = mqttPassword,
            clientId = TAG,
            receiver = mqttReceiver
        )

        mMqttClient.connect(listOf(TOPIC, TOPIC_DEPTH))
    }

    override fun onPause() {
        super.onPause()
        mMqttClient.destroy()
    }

}