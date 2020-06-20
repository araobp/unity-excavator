# Unity excavator

This project is a cumulation of my weekend works on Unity, to operate a 3D model excavator on Unity.

![scene](./scene.png)

Note: I am working on Windows10 for this creation. I have used Blender to create the 3D model of excavator.

## Requirements

- Unity 2018.4
- Standard Assets
- Optional: Logicool Gamepad F310

## Operation

### PC keyboard

```

[Travel levers]
O: Right track reverse
U: Right track forward
Y: Left track forward
R: Left track reverse

      RTUYIOP
      FGH LJI
  
[Operation levers]
I: Boom roll in
K: Boom roll out
L: Bucket roll out
J: Bucket roll in
T: Arm roll out
G: Arm roll in
H: Swing right
F: Swing left

```

### Logicool Gamepad F310

Use the left and right joysticks. Push B button to switch between the operation lever mode and the travel lever mode.

## Cameras

The excavator is equipped with four cameras:
- operator view
- three rear cameras (initially disabled)

The rear cameras support mirror view.

## Mathematics and Physics

I have applied IK to bucket positioning: Euler angles at the boom joint and the arm joint are caluculated based on Cosine Theorem.

I attached Rigidbody and colliders to the excavator with Gravity enabled.

TODO: Autonoumous driving is to be supported soon.

## Working with point cloud

It is possible to replace the virtual terrain with real terrain by importing poing cloud data.

I use [CloudCompare](https://www.danielgm.net/cc/) to convert LAS poing cloud data to FBX, then use Unity's Raycast API to convert FBX mesh to Terrain.

## The reason why I open this project

I am migrating from Unity to Unreal Engine for some reasons, but I continue using Unity on weekend works as my hobby. Unity and C#/VisualStudio are greate tools for creating 3D mobile apps.
