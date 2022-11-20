# Drone remote controller for iPad

Not only for iPad but also for other tablet/smartphone models.

However, iPad mini is the best for this remote controller emulation in terms of the size.

## Package dependencies

- Input System
- URP

## Joypad emulation on iPad mini

<img src="DroneRemoteController1.jpg" width=500>

<img src="DroneRemoteController2.jpg" width=500>

## Code

=> [Code](../DroneRemoteController) 


I devised the following part in "StickController.cs" to stabilize the stick position:

```
    public float deltaX
    {
        get => Mathf.Sign(m_DeltaX) * Mathf.Pow(m_DeltaX, 2);
    }

    public float deltaY
    {
        get => Mathf.Sign(m_DeltaY) * Mathf.Pow(m_DeltaY, 2);
    }
```

## Camera attitude control

I use InputSystem's AttitudeSensor for controlling camera attitude.

The following code is to support both remote controller emulation and attitude control:

```
    float m_RotationY = 0F;

         :
         
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Stick Control
        float deltaYLeft = m_StickControllerLeft.deltaY * deltaTime * m_MultiplierTranslate;
        float deltaXRight = m_StickControllerRight.deltaX * deltaTime * m_MultiplierRotate;
        float deltaYRight = m_StickControllerRight.deltaY * deltaTime * m_MultiplierTranslate;
        Transform cam = m_CameraObject.transform;
        cam.Translate(new Vector3(20F * deltaXRight, 20F * deltaYLeft, 40F * deltaYRight));

        float deltaXLeft = m_StickControllerLeft.deltaX * deltaTime * m_MultiplierTranslate;
        m_RotationY += 100F * deltaXLeft;

        // Camera attitude Control
        Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
        attitude = Quaternion.Euler(0, 0, -180) * Quaternion.Euler(-90, 0, 0) * attitude * Quaternion.Euler(0, 0, 180);
        cam.transform.rotation = attitude;// * Quaternion.AngleAxis(m_RotationY, Vector3.up);

        // Rotate the camera around Y-axis in world splace
        cam.Rotate(new Vector3(0F, m_RotationY, 0F), Space.World);
    }
}
