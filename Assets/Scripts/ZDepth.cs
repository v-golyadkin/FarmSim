using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZDepth : MonoBehaviour
{
    Transform t;
    [SerializeField] bool stationary = true;

    public void Start()
    {
        t = transform;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = pos.y * 0.00001f;
        transform.position = pos;

        if(stationary) { Destroy(this); }
    }
}
