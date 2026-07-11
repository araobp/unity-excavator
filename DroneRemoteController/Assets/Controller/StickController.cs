using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StickController : MonoBehaviour
{
    [SerializeField]
    bool _IsLeftStick = false;

    Rect _StickArea;
    Transform _Stick;

    float _WidthHalf;
    float _HeightHalf;
    float _DeltaX;
    float _DeltaY;

    bool _IsDraggingWithMouse = false;

    void OnEnable()
    {
        // Enable Enhanced Touch Support when the script is enabled
        EnhancedTouchSupport.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform of the stick area and calculate its dimensions
        Rect rect = transform.Find("PanelStickArea").gameObject.GetComponent<RectTransform>().rect;
        // Calculate half-width and half-height for stick movement limits
        _WidthHalf = rect.width / 2;
        // Calculate half-height for stick movement limits
        _HeightHalf = rect.height / 2;
        // Define the stick area rectangle for touch detection
        _StickArea = new Rect(-_WidthHalf, -_HeightHalf, rect.width, rect.height);

        // Find the stick transform for moving the stick based on touch input
        _Stick = transform.Find("RawImageStick");

        // Automatically determine if this is the left or right stick based on name if not set
        if (gameObject.name.ToLower().Contains("left"))
        {
            _IsLeftStick = true;
        }
        else if (gameObject.name.ToLower().Contains("right"))
        {
            _IsLeftStick = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool stickTouched = false;
        var count = Touch.activeTouches.Count;
        if (count > 0)
        {
            var touches = Touch.activeTouches;
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = touches[i].screenPosition;
                Vector3 localPos = transform.InverseTransformPoint(pos);
                if (_StickArea.Contains(localPos))
                {
                    _DeltaX = localPos.x;
                    _DeltaY = localPos.y;
                    if (_DeltaX > _WidthHalf) _DeltaX = _WidthHalf;
                    if (_DeltaX < -_WidthHalf) _DeltaX = -_WidthHalf;
                    if (_DeltaY > _HeightHalf) _DeltaY = _HeightHalf;
                    if (_DeltaY < -_HeightHalf) _DeltaY = -_HeightHalf;

                    _Stick.localPosition = new Vector2(_DeltaX, _DeltaY);

                    _DeltaX /= _WidthHalf;
                    _DeltaY /= _HeightHalf;

                    stickTouched = true;
                }
            }
        }
        else
        {
            // Mouse Dragging Fallback: simulate touch stick drag with standard mouse input
            if (Mouse.current != null)
            {
                // When mouse is clicked down inside the stick area, mark that we are dragging
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector3 mousePos = Mouse.current.position.ReadValue();
                    Vector3 localPos = transform.InverseTransformPoint(mousePos);
                    if (_StickArea.Contains(localPos))
                    {
                        _IsDraggingWithMouse = true;
                    }
                }

                // If currently dragging, update stick localPosition relative to the stick area
                if (_IsDraggingWithMouse)
                {
                    if (Mouse.current.leftButton.isPressed)
                    {
                        Vector3 mousePos = Mouse.current.position.ReadValue();
                        Vector3 localPos = transform.InverseTransformPoint(mousePos);
                        
                        _DeltaX = localPos.x;
                        _DeltaY = localPos.y;
                        if (_DeltaX > _WidthHalf) _DeltaX = _WidthHalf;
                        if (_DeltaX < -_WidthHalf) _DeltaX = -_WidthHalf;
                        if (_DeltaY > _HeightHalf) _DeltaY = _HeightHalf;
                        if (_DeltaY < -_HeightHalf) _DeltaY = -_HeightHalf;

                        _Stick.localPosition = new Vector2(_DeltaX, _DeltaY);

                        _DeltaX /= _WidthHalf;
                        _DeltaY /= _HeightHalf;

                        stickTouched = true;
                    }
                    else
                    {
                        // Stop dragging when left button is released
                        _IsDraggingWithMouse = false;
                    }
                }
            }

            // Keyboard Fallback: map Arrow keys for Left Stick (Yaw/Elevation) and WASD keys for Right Stick (Strafe/Surge)
            if (!stickTouched && Keyboard.current != null)
            {
                float keyX = 0f;
                float keyY = 0f;

                if (_IsLeftStick)
                {
                    if (Keyboard.current.leftArrowKey.isPressed) keyX = -1f;
                    if (Keyboard.current.rightArrowKey.isPressed) keyX = 1f;
                    if (Keyboard.current.upArrowKey.isPressed) keyY = 1f;
                    if (Keyboard.current.downArrowKey.isPressed) keyY = -1f;
                }
                else
                {
                    if (Keyboard.current.aKey.isPressed) keyX = -1f;
                    if (Keyboard.current.dKey.isPressed) keyX = 1f;
                    if (Keyboard.current.wKey.isPressed) keyY = 1f;
                    if (Keyboard.current.sKey.isPressed) keyY = -1f;
                }

                // If any key is pressed, map them directly to the stick coordinates
                if (keyX != 0f || keyY != 0f)
                {
                    _DeltaX = keyX * _WidthHalf;
                    _DeltaY = keyY * _HeightHalf;
                    _Stick.localPosition = new Vector2(_DeltaX, _DeltaY);
                    _DeltaX = keyX;
                    _DeltaY = keyY;
                    stickTouched = true;
                }
            }
        }

        if (!stickTouched)
        {
            _DeltaX = 0;
            _DeltaY = 0;
            _Stick.localPosition = Vector2.zero;
        }
    }
    
    public float deltaX
    {
        get => Mathf.Sign(_DeltaX) * Mathf.Pow(_DeltaX, 2);
    }

    public float deltaY
    {
        get => Mathf.Sign(_DeltaY) * Mathf.Pow(_DeltaY, 2);
    }
}


