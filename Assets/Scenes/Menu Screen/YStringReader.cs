using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YStringReader : MonoBehaviour
{
    private string input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Screen.SetResolution(Screen.width, Convert.ToInt32(s), true);
    }
}
