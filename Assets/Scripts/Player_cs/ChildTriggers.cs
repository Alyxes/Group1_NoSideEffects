using UnityEngine;

namespace NoSideEffects
{
    public class ChildTrigger : MonoBehaviour
    {
        public TaskSwitch parentTrigger;
        public string triggerName;

        private GameObject triggerCollider;

        private void Awake()
        {
            triggerCollider = GetComponent<GameObject>();
            if (triggerCollider == null)
                Debug.LogError("No Collider found on ChildTrigger!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggerCollider.activeSelf) return;

            if (other.CompareTag("InteractZone"))
            {
                parentTrigger.InTaskZone = true;
                parentTrigger.PlayerEntered(triggerName);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!triggerCollider.activeSelf) return;

            if (other.CompareTag("InteractZone"))
            {
                parentTrigger.InTaskZone = false;
                parentTrigger.PlayerExited(triggerName);
            }
        }

        // <-- New method to enable/disable the trigger
        public void SetTriggerActive(bool isActive)
        {
            if (triggerCollider != null)
                triggerCollider.SetActive(false);
        }
    }
}
