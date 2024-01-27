using GateClosing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public static Dictionary<Collider, Ragdoll> Ragdolls = new();
    public static Dictionary<Collider, Player> Player = new();
}
