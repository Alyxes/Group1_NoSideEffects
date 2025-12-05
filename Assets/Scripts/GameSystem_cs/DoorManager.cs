using NUnit.Framework;
using UnityEngine;


namespace NoSideEffects
{
    public class DoorManager : MonoBehaviour
    {
        public static DoorManager instance;

        public Transform bedroomDoor;
        public Transform bathroomDoor;
        public Transform livingroomDoor;
        public Transform frontDoor;
        public Transform strangeDoor;
        public Transform fridgeDoor;
        public Transform kitchenHatch1;
        public Transform kitchenHatch2;
        public Transform kitchenHatch3;
        public Transform kitchenHatch4;
        public Transform cleaningClosetDoor;
        public Transform closetDoor1;
        public Transform closetDoor2;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            
        }

        public void SetDoorRotation(Transform door, float yRotation)
        {
            if (door != null)
                door.rotation = Quaternion.Euler(0, yRotation, 0);
            else
                Debug.LogWarning("Door Transform is not assigned.");
        }
        public void SetDoorClosed(Transform door)
        {
            if (door != null)
                door.rotation = Quaternion.Euler(0, 0, 0);
            else
                Debug.LogWarning("Door Transform is not assigned.");
        }
        public void SetDoorOpen(Transform door)
        {
            if (door != null)
                door.rotation = Quaternion.Euler(0, 135f, 0);
            else
                Debug.LogWarning("Door Transform is not assigned.");
        }

    }
}