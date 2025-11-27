using UnityEngine;

namespace NoSideEffects
{
    public class PlayerInventory : MonoBehaviour
    {
        // Currently held item (static so ItemPickup can access it)
        public static ItemPickup currentHeldItem;
        // Hold point for attaching items in player's hand
        public static Transform holdPoint;
        [Header("Hold Point Reference")]
        [SerializeField] private Transform holdPointReference; // Assign in Inspector
        public string HeldItemID
        {
            get
            {
                if (currentHeldItem != null)
                    return currentHeldItem.itemID;
                else
                    return null;
            }
        }

        private void Awake()
        {
            // Assign static hold point from inspector reference
            if (holdPointReference != null)
            {
                holdPoint = holdPointReference;
            }
            else
            {
                Debug.LogError("PlayerInventory: HoldPoint not assigned in Inspector!");
            }
        }
    }
}
