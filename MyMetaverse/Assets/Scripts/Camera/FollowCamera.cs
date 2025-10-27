using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private bool followX;
    [SerializeField] private bool followY;
    [SerializeField] private Transform target;

    private float offsetX;
    private float offsetY;

    private void Start()
    {
        offsetX = transform.position.x - target.position.x;
        offsetY = transform.position.y - target.position.y;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        float distanceX = target.position.x - transform.position.x + offsetX;
        float distanceY = target.position.y - transform.position.y + offsetY;

        if(followX)
            transform.position += new Vector3(distanceX, 0, 0);

        if (followY)
            transform.position += new Vector3(0, distanceY, 0);
    }
}
