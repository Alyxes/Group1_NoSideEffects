using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

namespace NoSideEffects
{
    public class day3puzzle : MonoBehaviour
    {
        public InfoZone triggerZone;
        public Transform doorPivot;
        public GameObject puzzledApartment;
        public GameObject normalStartApartment;
        public GameObject normalApartmentAfter;
        private bool isPuzzleOn, isPuzzleDone = false;
        void Awake()
        {
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
        }
        void Update()
        {
            if (isPuzzleDone)
                return;

            if (!isPuzzleOn && triggerZone != null && triggerZone.hasBeenChecked)
            {
                isPuzzleOn = true;
                HUD.instance.ClearPickUpText();
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
                    doorPivot.rotation = Quaternion.Euler(0, 0, 0);
                    Debug.Log("DOOR SLAM");
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
            StartCoroutine(HUD.instance.SetTimerUntilSubTitle(2f, "Woah! What's going on!?"));
        }
        private IEnumerator RemoveDay3Puzzle()
        {
            yield return new WaitForSeconds(1f);
            normalApartmentAfter.SetActive(true);
            puzzledApartment.SetActive(false);
        }
    }
}