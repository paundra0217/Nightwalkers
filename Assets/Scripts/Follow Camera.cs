using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float SmoothSpeed = 0.25f;
    [SerializeField] Vector3 Offset = new Vector3(0, 0, -10f);
    public Transform Target;

    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 newpos = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, newpos, ref velocity, SmoothSpeed);



        //transform.LookAt(Target);
    }

}
