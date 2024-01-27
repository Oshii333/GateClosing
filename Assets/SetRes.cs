using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRes : MonoBehaviour
{
    public void resolutionSet(int inputResolution) {
        Screen.SetResolution(inputResolution, Screen.height, true);
    }
}
