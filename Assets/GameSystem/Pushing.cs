using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Pushing : MonoBehaviour
    {
        private InputAction interactButton;
        public GameObject testObject;

        private void DisableKinematic()
        {
            testObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        private void EnableKinematic()
        {
            testObject.GetComponent<Rigidbody>().isKinematic = true;
        }


        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}