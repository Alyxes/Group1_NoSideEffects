using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Pushing : MonoBehaviour
    {
        private InputAction interactButton;
        private Collider triggerCollider;
        private bool inZone = false;

        private void DisableKinematic()
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        private void EnableKinematic()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }


        void Start()
        {
            triggerCollider = GetComponent<Collider>();
            interactButton = InputSystem.actions.FindAction("Interact");
        }

        // Update is called once per frame
        void Update()
        {
            if (interactButton.WasPressedThisFrame())
            {
                if (GetComponent<Rigidbody>().isKinematic == true)
                {
                    DisableKinematic();
                }
                else if(GetComponent<Rigidbody>().isKinematic == false)
                {
                    EnableKinematic();
                }
                    Debug.Log("button pressed");
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PushZone"))
            {
                if (!inZone)
                {
                    inZone = true;
                    Debug.Log("Is in pushing zone");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PushZone"))
            {
                if (inZone)
                {
                    inZone = false;
                    Debug.Log("left the pushing zone");
                }
            }
        }
    }
}