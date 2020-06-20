# unity-excavator

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

## Mathematics and Physics

I have applied IK to bucket positioning: Euler angles at the boom joint and the arm joint are caluculated based on Cosine Theorem.

I attached Rigidbody and colliders to the excavator with Gravity enabled.

TODO: Autonoumous driving is to be supported soon.
