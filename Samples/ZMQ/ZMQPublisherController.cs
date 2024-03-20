using System.Collections;
using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using System;

/* A simple example of how to use ZMQ in Unity.
ZMQ is broadly recommended for low-frequency, high-channel-count data streams such as camera streams -- each pixel can be thought of as a channel.
This MonoBehavior-derived class can have all default Unity methods (Start, Update, etc.) be called automatically by Unity.

In this example, we open a ZMQ PublisherSocket and fire up a coroutine, UploadCapture, to broadcast camera images at a specified rate.
In each iteration of the coroutine, UploadCapture, we Send some Frames of arbitrary camera data using byte[]s (byte arrays).
In one line of code, you can chain any number of SendMoreFrame(bytes) as long as there is a final SendFrame(bytes) at the end.
Since ZMQ uses sockets and has more going on under the hood than LSL, remember to include a cleanup function such as OnDestroy(). */
public class ZMQPublisherController : MonoBehaviour
{
    [Header("Scene References")]
    public Camera captureCamera;  // in your editor, set this to the camera you want to capture

    [Header("ZMQ Camera Capture Settings")]
    public int imageWidth = 400;
    public int imageHeight = 400;
    public float sendRate = 15f;

    [Header("ZMQ Networking Settings")]
    public string tcpAddress = "tcp://localhost:5557";
    public string streamName = "unity_zmq_my_stream_name";

    [Header("Stream Status")]
    public PublisherSocket socket;
    public long imageCounter = 0;

    // objects to hold the image data;
    RenderTexture tempRenderColorTexture;
    Texture2D colorImage;

    /// Start is called before the first frame update
    private void Start()
    {
        // RenderTexture is basically a block of memory (NativeArray<T> under the hood) to manually capture the camera's image later on
        tempRenderColorTexture = new RenderTexture(imageWidth, imageHeight, 32, RenderTextureFormat.ARGB32)
        {
            antiAliasing = 4
        };

        // Texture2D is the actual image data that we will send over ZMQ
        // Notice that we drop the Alpha channel (TextureFormat.RGB24 here vs RenderTextureFormat.ARGB32 in tempRenderColorTexture since Alpha doesn't make sense for a camera image)
        // In order to read this stream in PhysioLabXR, pick the "uint8" data type so that each block of 24 bits is interpreted as a 3 8-bit channels (RGB)!
        colorImage = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false, true);

        ForceDotNet.Force();
        socket = new PublisherSocket(tcpAddress);
        StartCoroutine(UploadCapture(1f / sendRate));
    }

    /// <summary>
    /// A coroutine that uploads an image from captureCamera every waitTime seconds.
    /// </summary>
    IEnumerator UploadCapture(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            double timestamp = Time.unscaledTime;
            byte[] imageBytes = EncodeColorCamera();
            socket.SendMoreFrame(streamName).SendMoreFrame(BitConverter.GetBytes(timestamp)).SendFrame(imageBytes);
            imageCounter++;
        }
    }

    /// <summary>
    /// Encodes captureCamera's image into a byte array.
    /// </summary>
    public byte[] EncodeColorCamera()
    {
        // In order to render the camera manually to tempRenderColorTexture, we must change the target texture of the camera to tempRenderColorTexture
        // targetTexture is a pointer to a RenderTexture, again a continuous block of memory optimized for the GPU, to which the camera will render
        RenderTexture prevTargetTexture = captureCamera.targetTexture; // First, save the original target (we expect this will be the screen of the application, so we want to return it after we are done!)
        captureCamera.targetTexture = tempRenderColorTexture;
        RenderTexture.active = tempRenderColorTexture;
        captureCamera.Render(); // Manually Render() once to our target, capturing the camera's image!

        colorImage.ReadPixels(new Rect(0, 0, colorImage.width, colorImage.height), 0, 0);
        colorImage.Apply();

        captureCamera.targetTexture = prevTargetTexture; // Before returning, we must reset the camera's target texture to its original value to continue rendering to the screen
        return colorImage.GetRawTextureData(); // Finally, we now return the byte[] of the image!
    }

    private void OnDestroy()
    {
        socket.Dispose();
        NetMQConfig.Cleanup();
    }

}