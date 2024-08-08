using System;
using System.Collections;
using Cysharp.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsyncExampleClient : MonoBehaviour
{
    public Button Button;
    public TMP_Text Text;
    public TMP_InputField userInputField;
    
    private GrpcChannel channel;
    private YetAnotherHttpHandler handler;
    private AsyncRPCExample.AsyncRPCExampleClient client;

    public string host = "http://localhost:13004";

    // Start is called before the first frame update
    void Start()
    {
        Button.onClick.AddListener(OnCallRPC);
        
        // Set up the RPC client
        handler = new YetAnotherHttpHandler(){Http2Only = true};  // GRPC requires HTTP/2
        channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions() { HttpHandler = handler, Credentials = ChannelCredentials.Insecure});
        client = new AsyncRPCExample.AsyncRPCExampleClient(channel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnCallRPC()
    {
        Debug.Log("Calling RPC");
        
        StartCoroutine(CallLongAsyncRPC("World"));

        Debug.Log("Sent RPC request, started waiting for response");
    }

    private IEnumerator CallLongAsyncRPC(string message)
    {
        var request = new AsyncOneArgOneReturnRequest() { Input0 = userInputField.text ?? " " };  // The argument cannot be an empty string
        var call = client.AsyncOneArgOneReturnAsync(request);  // The method name in the client is "<method name in RenaScript>Async"
        yield return new WaitUntil(() => call.ResponseAsync.IsCompleted);

        if (call.ResponseAsync.IsCompletedSuccessfully)
        {
            var response = call.ResponseAsync.Result;
            Text.text = $"[{DateTime.Now}] {response}";
        }
        else
        {
            Debug.LogError("gRPC call failed: " + call.ResponseAsync.Exception);
        }
    }

}
