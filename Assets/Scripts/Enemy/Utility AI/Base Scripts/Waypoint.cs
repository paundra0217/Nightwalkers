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

    public bool waypointAvail()
    {
        if (_wp.Count >= 0) 
            return true;
        else if (_wp[0] = null) 
            return false;

        return false;
    }
}
