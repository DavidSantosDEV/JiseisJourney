using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public GameObject FollowTarget;
    private void LateUpdate()
    {
        if (FollowTarget)
        {
            transform.position = new Vector3(FollowTarget.transform.position.x, FollowTarget.transform.position.y, -10) ;
        }
    }
}
