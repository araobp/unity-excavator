# Drone remote controller for iPad

Joypad emulation on iPad

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
