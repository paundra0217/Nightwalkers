using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    [SerializeField] private float speed = 1f;

    private int dir = 1;
    [SerializeField] private float waitTime;

    // Start is called before the first frame update
    private void Update()
    {
        Vector2 target = currMoveTarget();

        platform.position = Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)platform.position).magnitude;
        float distest = Vector2.Distance(platform.position, target);

        if (distest <= .05f)
        {
            dir *= -1;
            StartCoroutine(WaitNextPoint());
        }
    }

    Vector2 currMoveTarget()
    {
        if(dir == 1)
            return startPoint.position;
        else return endPoint.position;
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitTime);
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
