using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform CameraToLookAt;

    void LateUpdate()
    {
        // Rotate this object to face the camera horizontally
        transform.LookAt(new Vector3(CameraToLookAt.position.x, transform.position.y, CameraToLookAt.position.z));
    }
}
