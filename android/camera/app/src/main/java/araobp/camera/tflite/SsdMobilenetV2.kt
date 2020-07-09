package araobp.camera.tflite

import android.content.Context
import android.graphics.Bitmap
import android.graphics.Color
import android.graphics.Paint

class SsdMobileNetV2(context: Context, numThreads: Int = 2) {

    companion object {
        const val INPUT_SIZE = 300
        private const val TF_OD_API_IS_QUANTIZED = true
        private val TF_OD_API_MODEL_FILE = arrayOf("detect.tflite", "labelmap.txt")
    }

    private val detector: Classifier

    private val paint = Paint()

    init {
        paint.apply {
            style = Paint.Style.FILL_AND_STROKE
            color = Color.BLUE
            textSize = 20F
        }
        detector = TFLiteObjectDetectionAPIModel.create(
            context.assets,
            TF_OD_API_MODEL_FILE[0],
            TF_OD_API_MODEL_FILE[1],
            INPUT_SIZE,
            TF_OD_API_IS_QUANTIZED,
            numThreads
        )
    }

    fun recognizeImage(src: Bitmap): List<Classifier.Recognition?>? {
        check(src.width == INPUT_SIZE && src.height == INPUT_SIZE)
        return detector.recognizeImage( src )
    }
}