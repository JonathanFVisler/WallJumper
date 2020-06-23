using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    Transform followedObject;
    Vector3 pos;

    private void Start()
    {
        pos = new Vector3(followedObject.position.x, followedObject.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = followedObject.position.x;
        pos.y = followedObject.position.y;
        transform.position = pos;
    }
}
