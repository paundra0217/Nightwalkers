using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    List<Transform> _wp = new List<Transform>();

    private void Start()
    {
        _wp.AddRange(transform.GetComponentsInChildren<Transform>());
    }
}
