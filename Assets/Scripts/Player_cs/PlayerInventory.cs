using UnityEngine;

namespace NoSideEffects
{
    public class PlayerInventory : MonoBehaviour
    {
        // Right and Left hand items
        public static ItemPickup currentHeldItem;
        public static ItemPickup currentHeldItemLeft;

        public static Transform holdPoint;
        public static Transform holdPoint2;

        [SerializeField] private Transform holdPointReference;
        [SerializeField] private Transform holdPoint2Reference;

        [SerializeField] private TaskSwitch taskSwitch;

        public string HeldItemID => currentHeldItem != null ? currentHeldItem.itemID : null;
        public string HeldItemIDLeft => currentHeldItemLeft != null ? currentHeldItemLeft.itemID : null;

        private void Awake()
        {
            // Assign hold points
            if (holdPointReference != null) holdPoint = holdPointReference;
            if (holdPoint2Reference != null) holdPoint2 = holdPoint2Reference;

            // Ensure taskSwitch exists
            if (taskSwitch == null)
                taskSwitch = FindFirstObjectByType<TaskSwitch>();
        }

        public void PickUpItem(ItemPickup pickedUpItem, bool leftHand = false)
        {
            if (pickedUpItem == null) return;

            if (leftHand)
            {
                currentHeldItemLeft = pickedUpItem;
                if (holdPoint2 != null)
                {
                    pickedUpItem.transform.SetParent(holdPoint2);
                    pickedUpItem.transform.localPosition = Vector3.zero;
                    pickedUpItem.transform.localRotation = Quaternion.identity;
                }
                Debug.Log("Picked up (Left Hand): " + pickedUpItem.itemID);
            }
            else
            {
                currentHeldItem = pickedUpItem;
                if (holdPoint != null)
                {
                    pickedUpItem.transform.SetParent(holdPoint);
                    pickedUpItem.transform.localPosition = Vector3.zero;
                    pickedUpItem.transform.localRotation = Quaternion.identity;
                }
                Debug.Log("Picked up (Right Hand): " + pickedUpItem.itemID);
            }

            // Trigger task immediately
            if (taskSwitch == null)
                taskSwitch = FindFirstObjectByType<TaskSwitch>();

            taskSwitch?.StartTaskForCurrentItem();
        }

        public void DropItem(bool leftHand = false)
        {
            if (leftHand)
            {
                if (currentHeldItemLeft != null)
                {
                    currentHeldItemLeft.transform.SetParent(null);
                    Debug.Log("Dropped: " + currentHeldItemLeft.itemID + " (Left Hand)");
                    currentHeldItemLeft = null;
                }
            }
            else
            {
                if (currentHeldItem != null)
                {
                    currentHeldItem.transform.SetParent(null);
                    Debug.Log("Dropped: " + currentHeldItem.itemID + " (Right Hand)");
                    currentHeldItem = null;
                }
            }
        }
    }
}
