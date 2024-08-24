# Procedural_Bug_Movement

This Unity project implements a procedural body and leg controller for a multi-legged character, like a spider or insect. The system dynamically aligns and moves the character’s body based on the terrain, while also allowing player control over movement and rotation.

https://github.com/user-attachments/assets/e71eae58-498e-4833-897d-bceaf9ab5074

## Features

- **Smart Leg Movement**: Legs adjust automatically to the ground, using UniTask and DOTween for smooth, jumping-like steps.
- **Body Alignment**: The body tilts and moves based on the legs’ positions, staying balanced even on slopes.
- **WASD Control**: Move forward, backward, and rotate easily with simple controls.
- **Customizable**: Adjust movement speed, rotation, and offsets to fit your needs.

## Requirements

- **Unity 6 (2022.1 or later)**
- **DOTween & UniTask**: To be able to simulate step movement(foot up-forward-down).

## Setup

1. Attach the `BodyController` script to your creature’s body.
2. Set up your legs in the Inspector.
3. Play and control your creature with WASD!
