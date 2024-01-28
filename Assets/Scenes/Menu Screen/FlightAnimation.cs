using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAnimation : MonoBehaviour
{
    public Rigidbody plane;
    Vector3 plane_euler;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        plane = GetComponent<Rigidbody>();
        plane.velocity = new Vector3(36, 0, 15);
        Invoke("stopPlane", 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void stopPlane()
    {
        plane.velocity = Vector3.zero;
        rotatePlane();
    }

    void rotatePlane()
    {
        transform.rotation = Quaternion.FromToRotation(transform.forward, Vector3.up);
        Invoke("planeLift", 2);
    }

    void planeLift()
    {
        MenuAudioManager.Instance.PlaySFX("Screaming");
        plane.velocity = new Vector3(-20, 20, -50);
    }
}
