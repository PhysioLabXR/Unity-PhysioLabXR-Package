/* This class is used to rotate the object on its own axis.
 * It is used in the AsyncRPC scene to rotate the cube, to show that the scene is running while the RPC is being executed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
