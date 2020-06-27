# Basic classical Physics

This project has multiple scenes:

- Free fall
- Pendulum
- Slope
- Vector
- Friction

I have confirmed that all the Physics experiments on Unity follow the lows of Classical Physics.

## Free fall

![freefall](./freefall.png)

## Pendulum

![pendulum](./pendulum.png)

## Slope

![slope](./twoSlopes.png)

## Vector

Realtime rendering of velocity vector.

![velocityVector](./velocityVector.png)

# Friction

Comparison between Unity's Physics and Physics in real world Physics.

![friction](./friction.png)

[Note] quote from Unity manual on the friction model:

Please note that the friction model used by the Nvidia PhysX engine is tuned for performance and stability of simulation, and does not necessarily present a close approximation of real-world physics. In particular, contact surfaces which are larger than a single point (such as two boxes resting on each other) will be calculated as having two contact points, and will have friction forces twice as big as they would in real world physics. You may want to multiply your friction coefficients by 0.5 to get more realistic results in such a case.


## Code

[=> Code](../BasicClassicalPhysics)

There are multiple scenes in "BasicClassicalPhysics\Assets\Scenes".
