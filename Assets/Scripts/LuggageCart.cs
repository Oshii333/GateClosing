using GateClosing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LuggageCart : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.up, Vector3.up, 30 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.velocity = transform.forward * 200;
    }
}
