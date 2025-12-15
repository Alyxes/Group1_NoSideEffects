using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [NonSerialized] public bool lockRotation, lookingAt = false;
    [NonSerialized]  public Transform lookAtObject;

    void FixedUpdate()
    {
        if (lookingAt)
            transform.LookAt(lookAtObject);

        if (lockRotation)
        {
            // Code that overrides cinemachine camera rotation goes here...

        }
    }

    public void SetLookAtObject(Transform lookAtThis)
    {
        lookAtObject = lookAtThis;
        lookingAt = true;
    }
    public void SetRotationLock(bool locked)
    {
        lockRotation = locked;
    }
}
