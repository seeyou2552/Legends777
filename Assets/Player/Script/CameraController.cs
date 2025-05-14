using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private float offsetX;
    private float offsetY;

    void Start()
    {
        offsetX = transform.position.x - target.position.x;
        offsetY = transform.position.y - target.position.y;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;
        pos.y = target.position.y + offsetY;
        transform.position = pos;
    }
}
