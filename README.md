# AutoWSD VR Driving Simulator

AutoWSD is a Virtual Reality driving simulator for HCI research. This project focuses on a multi-modal interaction and view management system for quickly prototyping HCI patterns for future intelligent vehicles.

Research Group: Mobile Interactive Systems (https://mint.fh-hagenberg.at/)

![AutoWSD Setup](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/Setting.jpg)

![AutoWSD Overview](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/Overview.png)

## Systems

### Vehicle System

The Vehicle System contains functionality for basic vehicle driving behavior (acceleration, braking, steering, engine sounds, colliders, physics, ...).

### Data Management System

#### DisplayModule

Submodules:
 - MultipleDisplays: for displaying the content of the scene on up to eight monitors (instead of using VR HMD). Each defined camera represents one monitor.
 - VideoPlayer: for showing a local (*.mp4) or online (e.g., YouTube) videos.
 - Webcam: for showing the webcam's video feed on the windshield display (if a webcam is connected).

#### InputModule

Submodules:
  - JSONLoader: for loading json files (Example: semantic sentences used for cognitive tasks).
  - TextToSpeech: for transforming predefined or runtime-created text into synthesized speech.
  - UDPInput: for handling UDP connections (used in combination with the [AutoWSD GUI Dashboard](https://github.com/mint-hagenberg/AutoWSD-Dashboard/)).

#### MessagingModule

Provides an EventManager responsible for passing data through the simulation.
Based on the subscriber pattern and emphasizes loose coupling between components.
Examples: 
 - When the user performs a certain gesture, cancel the current task.
 - When the vehicle arrives at a crossing, stop the vehicle.

#### OutputModule

Provides a helper that enables persistence of pre-defined data (e.g., take-over times) in one or multiple CSV files.
Should be used in conjunction with the UserStudyModule for further data processing using scenario and participant IDs.

#### PerformanceModule

Provides a helper to show how many frames per second the current scene is rendering in.
Orange or red FPS indicator signals that the current part of the scene might be too "heavy" for the VR system to render.

#### PostProcessingModule

Provides an example of using Unity's PostProcessing Stack for achieving high-quality renderings.

#### SoundModule

Provides a helper for managing sounds upon events, such as button clicks, start of simulation, crash, ...

#### TimingModule

Provides a countdown and elapsed timer for measuring task performance, take over times, ...

#### UserStudyModule

Provides an example of handling a user study with the possibility to enter scenario details (start screen, pause screen, questionnaire, ...).
Can be used standalone or in conjunction with the [AutoWSD GUI Dashboard](https://github.com/mint-hagenberg/AutoWSD-Dashboard/).

### Interaction System

#### GazeModule

Currently, gaze is based on the user's head rotation.
Gaze is indicated by a white dot.
For users of HTC Vive Pro Eye, the gaze will be based on eye tracking and is currently in development.

#### GestureModule

Implemented using Leap Motion Orion.
Supported gestures include swiping up/down/left/right/forward/backward, fist, palm up/down and thumb up/down.

#### SpeechModule

Implemented using Microsoft Speech API (offline version).
Predefined keywords or phrases can be entered in advance and an event is fired upon recognition of the word(s).

#### SteeringWheelModule

Implemented using Logitech SteeringWheelSDK.
Tested on G24 and G29/920 steering wheels.
Pedals and steering wheels are only working in "Manual Driving" mode. In "Automated Driving" mode, the functionality of the steering wheel and pedals is deactivated.

### WSD System

Simulates a windshield display (WSD) or large head-up display (HUD).

![WSD HUD](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/WSD_HUD.png)

![WSD Warning](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/WSD_Warning.png)

### Traffic System

Defines a waypoint-based road environment that the automated vehicle follows.
Additionally, triggers are provided to simulate take-over requests, or other events upon arrival of the vehicle at the trigger location (using the EventManager).

![TOR](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/TORArea.png)

### Pedestrian System

Basic pedestrian walk paths including crossings, and example usage of Adobe Mixamo models and animations.

![TOR](https://raw.github.com/mint-hagenberg/AutoWSD/master/screenshots/Pedestrians.png)

### Weather System

Weather simulations such as fog, rain, snow, sun, and example usage of custom sky boxes.


---

## Installation and Setup

### Recommended Hardware

- GPU: Nvidia GeForce GTX 1070, RTX 2070
- RAM: 8 GB
- Disk Space: 3 GB
- VR HMD: HTC Vive (Pro)
- Leap Motion (only for gesture control, visualization of VR hands)
- Microphone (only for speech recognition)
- Logitech steering wheel (only for take-over scenarios, manual driving)

### Software

- OS: Windows 10
- Unity 2019.3+ (free version)
- Visual Studio/Rider for C# coding
- Nvidia GeForce Experience
- Steam and SteamVR
- Leap Motion Orion
- Logitech G Hub for the Logitech steering wheel

### Unity Project

- Clone/Download this project
- Open with Unity / Add to Unity Hub

### HTC Vive (Pro)

- Getting started:
  - https://www.vive.com/eu/product/vive-pro/
  - https://www.vive.com/eu/support/vive-pro-hmd/category_howto/setting-up-for-the-first-time.html

### Leap Motion

- Getting started:
  - https://www.ultraleap.com/product/leap-motion-controller/
  - https://developer.leapmotion.com/get-started

### Logitech Steering Wheel and Pedals

- Getting started:
  - https://www.logitechg.com/en-gb/products/driving/driving-force-racing-wheel.html
  - https://www.logitechg.com/en-gb/innovation/g-hub.html

---

## Environment

We want to keep this repository small, therefore we omitted the Windridge City Asset which we use for our city scenes.
You should download it from the Unity asset store (https://assetstore.unity.com/packages/3d/environments/roadways/windridge-city-132222) and place it in this directory: Assets/3rdPartyAssets/Environment/WindridgeCity

## Models/Animations

### Characters/Pedestrians can be created and imported using Adobe Fuse and Mixamo

- Use any humanoid character
- Upload FBX to https://www.mixamo.com
- Setup rigging
- Use any pose (e.g., walking)
- Download FBX file
- Import FBX file into Unity project

## Quantitative Data (Examples)

- Eye Gaze: where does the user look?
- Head Tracking: x/y/z position and rotation
- Gestures: hand gestures, finger gestures
- TOR (Take-over request) times
- Cognitive Task Performance (e.g., using semantic sentences -> error rate, speed, ...)

---

## GUI Dashboard

[AutoWSD GUI Dashboard](https://github.com/mint-hagenberg/AutoWSD-Dashboard/) is a simple Java GUI client for sending and receiving messages to and from this Unity instance.

### Screenshots

![Start simulation](https://raw.github.com/mint-hagenberg/AutoWSD-Dashboard/master/screenshots/AutoWSD_Dashboard_Start.png)

---

## Future Work

- Integration of [Unity Machine Learning Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) for the Traffic System
- Transition to HDRP (High Definition Render Pipeline) once terrain, glass shaders become more usable
- Integration of our scenarios once the results are published
- Integration of TraffSim traffic simulator for conducting traffic simulations, calculating time to collision, fuel consumption etc.
  - Details: http://nemo.fh-hagenberg.at/

---

## References

If this projects helps your research, feel free to cite us:
- Andreas Riegler, Andreas Riener, and Clemens Holzmann. 2019. AutoWSD: Virtual Reality Automated Driving Simulator for Rapid HCI Prototyping. In Proceedings of Mensch und Computer 2019 (MuC’19). Association for Computing Machinery, New York, NY, USA, 853–857. DOI: https://doi.org/10.1145/3340764.3345366
- Andreas Riegler, Andreas Riener, and Clemens Holzmann. 2019. Virtual Reality Driving Simulator for User Studies on Automated Driving. In Proceedings of the 11th International Conference on Automotive User Interfaces and Interactive Vehicular Applications: Adjunct Proceedings (AutomotiveUI ’19). Association for Computing Machinery, New York, NY, USA, 502–507. DOI: https://doi.org/10.1145/3349263.3349595
