package araobp.camera

import android.content.Context
import kotlin.math.roundToInt


class Properties(val context: Context) {

    companion object {
        const val PREFS_NAME = "camera"
    }

    var mqttServer = "localhost"
    var mqttUsername = "anonymous"
    var mqttPassword = "password"
    var depthCameraRange = 50F  // 50 meters

    init {
        load()
    }

    fun load() {
        val prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
        mqttServer = prefs.getString("mqttServer", "localhost").toString()
        mqttUsername = prefs.getString("mqttUsername", "anonymous").toString()
        mqttPassword = prefs.getString("mqttPassword", "password").toString()
        depthCameraRange = prefs.getInt("depthCameraRange", 50).toFloat()
    }

    fun save() {
        val editor = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE).edit()
        editor.putString("mqttServer", mqttServer)
        editor.putString("mqttUsername", mqttUsername)
        editor.putString("mqttPassword", mqttPassword)
        editor.putInt("depthCameraRange", depthCameraRange.roundToInt())
        editor.apply()
    }
}