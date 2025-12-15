using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [NonSerialized] public bool lockRotation, lookingAt = false;
    [NonSerialized] public Transform lookAtObject;
    [NonSerialized] public float wantedXrotation;
    [NonSerialized] public float wantedYrotation;

    [NonSerialized] float smoothTime = 5f;
    float xVelocity = 1f;
    float yVelocity = 1f;

    private void Awake()
    {
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
        Vector3 currentEuler = transform.eulerAngles;
        float currentX = currentEuler.x;
        float currentY = currentEuler.y;
        float newX = currentX;
        float newY = currentY;
        if (currentX != wantedXrotation)
            newX = Mathf.SmoothDampAngle(currentX, wantedXrotation, ref xVelocity, smoothTime, Mathf.Infinity, Time.fixedDeltaTime);
        if (currentY != wantedYrotation)
            newY = Mathf.SmoothDampAngle(currentY, wantedYrotation, ref yVelocity, smoothTime, Mathf.Infinity, Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(newX, newY, 0);
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
