using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Door : MonoBehaviour
    {
        public Transform doorTransform;
        public float currentRotation;
        public float openRotationAngle;
        public float closedRotationAngle;
        public float turnSpeed = 200f;
        public float wantedRotation;
        
        public bool open = true;
        private bool inZone = false;

        private InputAction interactButton;

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        void FixedUpdate()
        {
            if (open)
            {
                if (wantedRotation != openRotationAngle)
                    wantedRotation = openRotationAngle;
            }
            else
            {
                if (wantedRotation != closedRotationAngle)
                    wantedRotation = closedRotationAngle;
            }

            if (currentRotation != wantedRotation)
            {
                currentRotation = Mathf.MoveTowardsAngle(currentRotation, wantedRotation, turnSpeed * Time.fixedDeltaTime);
                doorTransform.rotation = Quaternion.Euler(0, currentRotation, 0);

                if (Mathf.Approximately(currentRotation, wantedRotation))
                {
                    currentRotation = wantedRotation;
                    if (!open)
                        AudioManager.PlaySound(SoundType.BUTTONCLICK);
                }
            }

            if (inZone && interactButton.WasPressedThisFrame())
            {
                if (open)
                {
                    ToggleDoor(false);
                    HUD.instance.SetUniqueItemText("Open door");
                }
                else
                {
                    ToggleDoor(true);
                    HUD.instance.SetUniqueItemText("Close door");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                Debug.Log("In zone");
                if (!inZone)
                {
                    inZone = true;

                    if (open)
                        HUD.instance.SetUniqueItemText("Close door");
                    else
                        HUD.instance.SetUniqueItemText("Open door");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                Debug.Log("In zone");
                if (inZone)
                {
                    inZone = false;
                    HUD.instance.ClearPickUpText();
                }
            }
        }
        public void ToggleDoor(bool newState)
        {
            open = newState;
        }
        public void SnapDoorRotation()
        {
            if (open)
                doorTransform.rotation = Quaternion.Euler(0, openRotationAngle, 0);
            else
                doorTransform.rotation = Quaternion.Euler(0, closedRotationAngle, 0);
        }
        public void SetDoorRotationSpeed(float speed)
        {
            turnSpeed = speed;
        }
        public void ResetDoorRotationSpeed()
        {
            turnSpeed = 200f;
        }
        public IEnumerator WaitAndToggleDoor(float timer, bool newOpen)
        {
            yield return new WaitForSeconds(timer);
            ToggleDoor(newOpen);
        }
    }
}
