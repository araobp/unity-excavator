# Unity excavator

This project is an achivement of my self-study on digital twin in my spare time at night and on weekends.

![scene](./doc/scene.png)

Note: I am working on Windows10 for this creation. I have used Blender to create the 3D models in this project.

## Demo videos

- [Excavator](https://www.youtube.com/watch?v=0X4c5gxU6-A)
- [Pendulum](https://www.youtube.com/watch?v=2AjkpGLnm74)
- [Solar system](https://www.youtube.com/watch?v=2z0K-X5a5Ss)

## Requirements

- Unity 2018.4
- [Standard Assets](https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2017-3-32351)
- Optional: [Logicool Gamepad F310](https://www.logitechg.com/en-us/products/gamepads/f310-gamepad.940-000110.html)

## My weekend works (incl. TODO)

### Excavator manual/autonomous operation

- [Excavator simulation](./doc/Excavator.md) => Completed!

### Classical Physics with Rigidbody on Unity

- [Basic Classical Physics (Dynamics) simulation on Unity](./doc/BasicClassicalPhysics.md) => Completed!

### Working with surveying data

- [Terrain height map manipulation](./doc/HeightMapManipulation.md)
- [Converting LAS point cloud data to Unity's Terrain object](./doc/PointCloud.md) => Completed!
- Lazer range finder simulation
- Total station simulation
- Drone survey simulation

### Camera/image

- [Depth camera simulation](./doc/DepthCamera.md) => Completed!
- Using Unity's camera for AI object detection with MobleNet on TensorFlow

### Cyber-physical

- Digital twin synchronizing with a real servo motor via real CAN bus

### Virtual IoT devices

- Transferring JPEG images from Unity's cameras to an Android app via MQTT messaging bus
- Controlling digital twin from a joystick simulation app via MQTT messaging bus
- Transferring data from virtual sensors on Unity to MQTT messaging bus over private LoRa.
