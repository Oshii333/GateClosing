using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringReader : MonoBehaviour
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
        Screen.SetResolution(Convert.ToInt32(s), Screen.height, true);
    }
}
