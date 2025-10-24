using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float offset;

    private void Start()
    {
        offset = transform.position.x - target.position.x;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        float distanceX = target.position.x - transform.position.x + offset;
        transform.position += new Vector3(distanceX, 0, 0);
    }
}
