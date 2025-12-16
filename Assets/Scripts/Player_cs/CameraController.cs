using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform CameraHead;
    [NonSerialized] public Transform lookAtObject;

    [NonSerialized] public bool lockRotation, lookingAt = false;
    [NonSerialized] public float originalXrotation;
    [NonSerialized] public float originalYrotation;
    [NonSerialized] public float wantedXrotation;
    [NonSerialized] public float wantedYrotation;

    [NonSerialized] float smoothTime = 5f;
    float xVelocity = 1f;
    float yVelocity = 1f;

    private void Awake()
    {
        originalXrotation = CameraHead.rotation.x;
        originalYrotation = CameraHead.rotation.y;
        wantedXrotation = transform.rotation.x;
        wantedYrotation = transform.rotation.y;
    }
    void FixedUpdate()
    {
        if (lookingAt)
            transform.LookAt(lookAtObject);

        if (lockRotation)
        {
            // Code that overrides cinemachine camera rotation goes here...

        }
        // Smoothly interpolate angles using SmoothDampAngle (handles wrap-around)
        // Use Time.fixedDeltaTime because this runs in FixedUpdate.
        //Vector3 currentEuler = transform.eulerAngles;
        //float currentX = currentEuler.x;
        //float currentY = currentEuler.y;
        //float newX = currentX;
        //float newY = currentY;
        //newX = Mathf.SmoothDampAngle(currentX, wantedXrotation, ref xVelocity, smoothTime, Mathf.Infinity, Time.fixedDeltaTime);
        //newY = Mathf.SmoothDampAngle(currentY, wantedYrotation, ref yVelocity, smoothTime, Mathf.Infinity, Time.fixedDeltaTime);

        //transform.localRotation = Quaternion.Euler(newX, newY, 0);
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
    public void SetHeadRotation(float angleX, float angleY)
    {
        wantedXrotation = angleX;
        wantedYrotation = angleY;
    }
}
