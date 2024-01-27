using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{

    public Rigidbody camera;
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Rigidbody>();
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        i++;
        if((i/40)%2 == 0)
        {
            if (i%40 < 20)
            {
                camera.velocity = new Vector3(0, i*0.05f, 0);
            }
        }
        Invoke("Delay", 1);
    }
}
