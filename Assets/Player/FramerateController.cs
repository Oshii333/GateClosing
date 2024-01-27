using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateController : MonoBehaviour
{
    public int targetFramerate = 60;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

}
