using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public List<Vector3> wayPoints;

    public int nextTarget;

    public float moveSpeed = 1;

    Vector3 dir;
    
    public float dist;

    void Start()
    {
        NextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isReturning)
        {
            transform.Translate(-dir * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(transform.position, startPos);
            if(dist <= 0.1)
            {
                isReturning = false;
            }
        } 
        else
        {
            transform.Translate(dir * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(transform.position, endPos);
            if (dist <= 0.1)
            {
                isReturning = true;
            }
        }/**/

        transform.Translate(dir * moveSpeed * Time.deltaTime);
        dist = Vector3.Distance(transform.position, wayPoints[nextTarget]);
        if (dist <= 0.2)
        {
            NextTarget();
        }
    }

    void NextTarget()
    {
        nextTarget++;
        if(nextTarget >= wayPoints.Count)
        {
            nextTarget = 0;
        }
        dir =  wayPoints[nextTarget] - transform.position;
        dir = dir.normalized;
    }
}
