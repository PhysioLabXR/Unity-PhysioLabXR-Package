using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

/* A simple example of how to use LSL in Unity.
LSL is broadly recommended for high-frequency, low-channel-count data streams such as 250Hz EEG.
This MonoBehavior-derived class can have all default Unity methods (Start, Update, etc.) be called automatically by Unity.

In this example, we set up a StreamInfo data object and a StreamOutlet stream object in Start().
In each Update() (or FixedUpdate()!), we broadcast an array of arbitrary data. */
public class LSLOutletController : MonoBehaviour
{
    [Header("LSL Stream Settings")]
    public LSL.channel_format_t channelFormat = LSL.channel_format_t.cf_float32;
    public string streamName = "unity_lsl_my_stream_name";
    public string streamType = "LSL";
    public int channelNum = 8;
    public float nominalSamplingRate = 100.0f;

    [Header("Stream Status")]
    public StreamOutlet streamOutlet;
    public float start_time;
    public float sent_samples = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;

        StreamInfo streamInfo = new StreamInfo(streamName,
                                                streamType,
                                                channelNum,
                                                nominalSamplingRate,
                                                channelFormat
                                                );
        streamOutlet = new StreamOutlet(streamInfo);
    }

    // Update is called once per frame update
    void Update()
    {
        float elapsed_time = Time.time - start_time;
        int required_samples = (int)(elapsed_time * nominalSamplingRate) - (int)sent_samples;

        for (int i = 0; i < required_samples; i++)
        {
            // you can also get the channel count from streamOutlet.info().channel_count()
            float[] randomArray = new float[channelNum];
            for (int j = 0; j < channelNum; j++)
            {
                randomArray[j] = Random.Range(0.0f, 1.0f);
            }
            // Data is broadcasted only when push_sample(data) is called
            streamOutlet.push_sample(randomArray);
        }
        sent_samples += required_samples;
    }
}
