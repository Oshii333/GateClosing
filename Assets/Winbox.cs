using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GateClosing;

public class Winbox : MonoBehaviour
{
    public static event Del Win;

    public void OnTriggerEnter(Collider other)
    {
        if (ColliderManager.Player.TryGetValue(other, out Player value))
        {
            Win?.Invoke();
        }
    }
}
