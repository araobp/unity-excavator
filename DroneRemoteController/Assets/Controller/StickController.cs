using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StickController : MonoBehaviour
{
    Rect m_StickArea;
    Transform m_Stick;

    float m_WidthHalf;
    float m_HeightHalf;
    float m_DeltaX;
    float m_DeltaY;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Rect rect = transform.Find("PanelStickArea").gameObject.GetComponent<RectTransform>().rect;
        m_WidthHalf = rect.width / 2;
        m_HeightHalf = rect.height / 2;
        m_StickArea = new Rect(-m_WidthHalf, -m_HeightHalf, rect.width, rect.height);

        m_Stick = transform.Find("RawImageStick");
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
                if (m_StickArea.Contains(localPos))
                {
                    m_DeltaX = localPos.x;
                    m_DeltaY = localPos.y;
                    if (m_DeltaX > m_WidthHalf) m_DeltaX = m_WidthHalf;
                    if (m_DeltaX < -m_WidthHalf) m_DeltaX = -m_WidthHalf;
                    if (m_DeltaY > m_HeightHalf) m_DeltaY = m_HeightHalf;
                    if (m_DeltaY < -m_HeightHalf) m_DeltaY = -m_HeightHalf;

                    m_Stick.localPosition = new Vector2(m_DeltaX, m_DeltaY);

                    m_DeltaX /= m_WidthHalf;
                    m_DeltaY /= m_HeightHalf;

                    stickTouched = true;
                }
            }
        }

        if (!stickTouched)
        {
            m_DeltaX = 0;
            m_DeltaY = 0;
            m_Stick.localPosition = Vector2.zero;
        }
    }
    
    public float deltaX
    {
        get => Mathf.Sign(m_DeltaX) * Mathf.Pow(m_DeltaX, 2);
    }

    public float deltaY
    {
        get => Mathf.Sign(m_DeltaY) * Mathf.Pow(m_DeltaY, 2);
    }
}


