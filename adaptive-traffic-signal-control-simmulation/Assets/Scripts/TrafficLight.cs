using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public bool on;

    public Material green;
    public Material red;

    public GameObject light;

    public void ChangeLight(bool state)
    {
        light.GetComponent<MeshRenderer>().material = state ? green : red;
    }
}
