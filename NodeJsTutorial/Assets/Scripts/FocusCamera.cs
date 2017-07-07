using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    public Vector3 dir;
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + dir;
        }
    }
}
