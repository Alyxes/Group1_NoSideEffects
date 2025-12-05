using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Pushing : MonoBehaviour
    {
        public bool IsMovable = true;
        private InputAction interactButton;
        private Collider triggerCollider;
        public bool inZone = false;
        public string furnitureName;

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
            
            if (interactButton.IsPressed() && inZone == true)
            {
                if (GetComponent<Rigidbody>().isKinematic == true)
                {
                    DisableKinematic();
                }
               
                    Debug.Log("button pressed");               
            }
            else
            {
                EnableKinematic();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;
                    HUD.instance.SetUniqueItemText("Push " + furnitureName);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (inZone)
                {
                    inZone = false;
                    Debug.Log("left the pushing zone");
                    HUD.instance.ClearPickUpText();
                }
            }
        }
    }
}