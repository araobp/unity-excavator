package araobp.camera.util

import kotlin.math.pow
import kotlin.math.roundToInt

// Round a float value to the first decimal place
fun Float.roundToTheNth(n: Int): Float {
    val magnify = 10F.pow(n)
    return (this * magnify).roundToInt()/magnify
}

