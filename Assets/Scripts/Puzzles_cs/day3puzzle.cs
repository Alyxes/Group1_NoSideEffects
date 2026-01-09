using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

namespace NoSideEffects
{
    public class day3puzzle : MonoBehaviour
    {
        public InfoZone triggerZone;
        public Door doorPivot;
        public GameObject puzzledApartment;
        public GameObject normalStartApartment;
        public GameObject normalApartmentAfter;
        public PlayerController player;
        private bool isPuzzleOn, isPuzzleDone = false;
        void Awake()
        {
            if (player == null)
                player = GetComponentInChildren<PlayerController>();

            if (doorPivot == null)
                doorPivot = GetComponentInChildren<Door>();

            puzzledApartment.SetActive(false);
            normalApartmentAfter.SetActive(false);

            if (triggerZone == null)
            {
                triggerZone = GetComponent<InfoZone>();
                if (triggerZone == null)
                    triggerZone = GetComponentInChildren<InfoZone>();
            }

            if (triggerZone == null)
                Debug.LogWarning($"{nameof(day3puzzle)}: {nameof(triggerZone)} (InfoZone) not found or assigned on {gameObject.name}");

            if (doorPivot == null)
                Debug.LogWarning($"{nameof(day3puzzle)}: {nameof(doorPivot)} (Door) not found as child/parent of {gameObject.name}. OnTriggerEnter will be ignored until you assign it.");
        }
        void Update()
        {
            if (isPuzzleDone)
                return;

            if (!isPuzzleOn && triggerZone != null && triggerZone.hasBeenChecked)
            {
                isPuzzleOn = true;
                HUD.instance.ClearPickUpText();
                player.ToggleCameraRotationOff();
                StartCoroutine(ActivateDay3Puzzle());
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (isPuzzleOn)
            {
                if (other.CompareTag("Player"))
                {
                    // And sounds here too.
                    doorPivot.turnSpeed = 500f;
                    doorPivot.ToggleDoor(false);
                    doorPivot.canBeOpened = false;
                    doorPivot.showInteractionText = false;

                    isPuzzleOn = false;
                    isPuzzleDone = true;

                    StartCoroutine(RemoveDay3Puzzle());
                }
            }
        }
        private IEnumerator ActivateDay3Puzzle()
        {
            // We can play sounds as well here.
            yield return new WaitForSeconds(1f);
            
            puzzledApartment.SetActive(true);
            normalStartApartment.SetActive(false);
            StartCoroutine(HUD.instance.SetTimerUntilSubTitle(1f, "Woah! What's going on!?", true));
            player.ToggleCameraRotationOn();
        }
        private IEnumerator RemoveDay3Puzzle()
        {
            yield return new WaitForSeconds(1.5f);
            normalApartmentAfter.SetActive(true);
            puzzledApartment.SetActive(false);
        }
    }
}