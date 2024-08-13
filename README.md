# PhysioLabXR Package for Unity
The PhysioLabXR Plugin provides seamless integration of [LSL (Lab Streaming Layer)](https://labstreaminglayer.readthedocs.io/info/intro.html), 
[ZMQ (ZeroMQ)](https://zeromq.org/), and [gRPC](https://grpc.io/) communication 
protocols into Unity projects. This enables Unity developers to easily send and receive data, and call remote procedural calls (RPCs)
between Unity and the PhysioLabXR environment, facilitating real-time, multi-modal, brain-computer interfaces, and extended reality experiments.

## How to use this plugin
To add this plugin to your Unity project, go to Window->Package Manager and click the plus button at the top-left corner. Select 'Add package from git URL' from the dropdown. Add this link [https://github.com/PhysioLabXR/Unity-PhysioLabXR-Package.git](https://github.com/PhysioLabXR/Unity-PhysioLabXR-Package.git).
For more details, refer to [the docs](https://physiolabxrdocs.readthedocs.io/en/latest/LSLZMQUnityPackage.html).

## Features
LSL Support: Stream and receive real-time data with Lab Streaming Layer, perfect for neurofeedback and BCI applications.
ZMQ Integration: Use ZeroMQ for high-performance asynchronous messaging in distributed or concurrent applications.
gRPC Communication: Implement efficient, low-latency remote procedure calls (RPCs) with gRPC, ideal for scalable microservices and cloud-based apps.

## How to edit the plugin
1. Clone the repo, and open it in Unity.
2. To edit the sample scenes, rename 'sample'

## Documentation
Please find the complete docs on how to use this package [here](https://physiolabxrdocs.readthedocs.io/en/latest/LSLZMQUnityPackage.html).
