using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Transform> _wp = new List<Transform>();

    private void Start()
    {
        if (_wp.Count < 1)
        {
            return;
        }
        //_wp.AddRange(transform.GetComponentsInChildren<Transform>());
    }

    public bool waypointAvail()
    {
        if (_wp.Count >= 0) 
            return true;
        else if (_wp[0] = null) 
            return false;

        return true;
    }

    public Transform getPatrolPos(int index)
    {
        return _wp[index].transform;
    }

    public int wayPointCount()
    {
        return _wp.Count;
    }
}
