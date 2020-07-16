package araobp.camera

import android.app.Dialog
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.util.Log
import android.widget.EditText
import kotlinx.android.synthetic.main.activity_main.*
import kotlin.math.roundToInt

class MainActivity : AppCompatActivity() {

    companion object {
        val TAG: String = this::class.java.simpleName
        const val MQTT_SERVER = "mqttServer"
        const val MQTT_USERNAME = "mqttUsername"
        const val MQTT_PASSWORD = "mqttPassword"
        const val DEPTH_CAMERA_RANGE = "depthCameraRange"
    }

    private val mProps: Properties by lazy {
        Properties(this)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        // Settings dialog
        buttonSettings.setOnClickListener {

            mProps.load()

            val dialog = Dialog(this)
            dialog.setContentView(R.layout.settings)

            val editTextMqttServer = dialog.findViewById<EditText>(R.id.editTextMqttServer)
            editTextMqttServer.setText(mProps.mqttServer)

            val editTextMqttUsername = dialog.findViewById<EditText>(R.id.editTextMqttUsername)
            editTextMqttUsername.setText(mProps.mqttUsername)

            val editTextMqttPassword = dialog.findViewById<EditText>(R.id.editTextMqttPassword)
            editTextMqttPassword.setText(mProps.mqttPassword)

            val editTextDepthCameraRange = dialog.findViewById<EditText>(R.id.editTextDepthCameraRange)
            editTextDepthCameraRange.setText(mProps.depthCameraRange.roundToInt().toString())

            editTextMqttServer.addTextChangedListener(object : TextWatcher {
                override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) =
                    Unit

                override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) = Unit
                override fun afterTextChanged(p0: Editable?) {
                    mProps.mqttServer = editTextMqttServer.text.toString()
                }
            })

            editTextMqttUsername.addTextChangedListener(object : TextWatcher {
                override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) =
                    Unit

                override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) = Unit
                override fun afterTextChanged(p0: Editable?) {
                    mProps.mqttUsername = editTextMqttUsername.text.toString()
                }
            })

            editTextMqttPassword.addTextChangedListener(object : TextWatcher {
                override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) =
                    Unit

                override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) = Unit
                override fun afterTextChanged(p0: Editable?) {
                    mProps.mqttPassword = editTextMqttPassword.text.toString()
                }
            })

            editTextDepthCameraRange.addTextChangedListener(object : TextWatcher {
                override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) =
                    Unit

                override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) = Unit
                override fun afterTextChanged(p0: Editable?) {
                    try {
                        mProps.depthCameraRange = editTextDepthCameraRange.text.toString().toFloat()
                    } catch (e: Exception) {
                        Log.d(TAG, e.toString())
                    }
                }
            })

            dialog.setOnDismissListener {
                mProps.save()
            }

            dialog.show()
        }

        // Safie PTZ
        buttonPtzCamera.setOnClickListener {
            val intent = Intent(this, PtzCameraActivity::class.java).apply {
                putExtra(MQTT_SERVER, mProps.mqttServer)
                putExtra(MQTT_USERNAME, mProps.mqttUsername)
                putExtra(MQTT_PASSWORD, mProps.mqttPassword)
                putExtra(DEPTH_CAMERA_RANGE, mProps.depthCameraRange)
            }
            startActivity(intent)
        }

    }

}
