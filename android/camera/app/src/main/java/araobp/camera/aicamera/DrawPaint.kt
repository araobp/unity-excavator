package araobp.camera.aicamera

import android.graphics.Color
import android.graphics.Paint

/*** Color defenition ***/
val BLACK = Color.rgb(0, 0, 0)
val WHITE = Color.rgb(255, 255, 255)
val RED = Color.rgb(255, 0, 0)
val BLUE = Color.rgb(0, 0, 255)
val YELLOW = Color.rgb(255, 255, 0)
val CYAN = Color.rgb(0, 255, 255)
val GRAY = Color.rgb(128, 128, 128)
val LIGHT_GRAY = Color.rgb(211, 211, 211)
val MAROON = Color.rgb(128, 0, 0)
val OLIVE = Color.rgb(128, 128, 0)
val GREEN = Color.rgb(0, 255, 0)
val PURPLE = Color.rgb(128, 0, 128)
val ORANGE = Color.rgb(255, 165, 0)
val CORAL = Color.rgb(255, 127, 80)
val LIGHT_STEEL_BLUE = Color.rgb(176, 196, 222)
val SANDY_BROWN = Color.rgb(244, 164, 96)
val TEAL = Color.rgb(0, 128, 128)
val PINK = Color.rgb(255, 192, 203)
val LAVENDER = Color.rgb(230, 230, 250)

val paint = Paint()

val boundingBoxColors = mapOf(
    "person" to GREEN,
    "car" to LIGHT_STEEL_BLUE)

fun paintBoundingBox(title: String): Paint {
    paint.apply {
        style = Paint.Style.STROKE
        strokeWidth = 4F
        if (title in boundingBoxColors.keys) {
            color = boundingBoxColors.getValue(title)
        } else {
            color = GRAY
        }
    }
    return paint
}

fun paintTitleBox(paintColor: Int): Paint {
    paint.apply {
        style = Paint.Style.FILL
        color = paintColor
        textSize = 25F
    }
    return paint
}