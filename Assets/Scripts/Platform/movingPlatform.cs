using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 1f;

    int dir = 1;

    // Start is called before the first frame update
    private void Update()
    {
        Vector2 target = currMoveTarget();

        platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)platform.position).magnitude;

        if (distance <= .1f) dir *= -1;
    }

    Vector2 currMoveTarget()
    {
        if(dir == 1)
            return startPoint.position;
        else return endPoint.position;
    }

    private void OnDrawGizmos()
    {
        if(platform!= null && startPoint!=null && endPoint!=null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }
}
