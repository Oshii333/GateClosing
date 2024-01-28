using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraPan : MonoBehaviour
{

    public Rigidbody camera;
    public float i;

    public static string dataStore;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Rigidbody>();
        i = 0;
        MenuAudioManager.Instance.PlayMusic("Menu Music");
    }

    // Update is called once per frame
    void Update()
    {
        if (i > 20) i = 0;
        i += Time.deltaTime;
        if (i % 20 < 10) {
            camera.velocity = new Vector3(0, 0.005f * (i+10), 0);
            transform.Rotate(Vector3.right * Time.deltaTime);
        } else
        {
            camera.velocity = new Vector3(0, -0.005f * i, 0);
            transform.Rotate(Vector3.left * Time.deltaTime);
        }
    }
}
