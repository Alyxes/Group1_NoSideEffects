using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoSideEffects
{
    // Day State Machine
    public class DSM : MonoBehaviour
    {
        public static DSM instance;
        public enum Days
        {
            Day1,
            Day2,
            Day3,
            Day4,
            Day5,
            Day6,
            Day7
        }
        public Days currentDay = Days.Day1;
        public Days nextDay = Days.Day1;
        [NonSerialized] public string wakeUpMonologue = "";
        [NonSerialized] public float monolougeTimer = 2f;
        [NonSerialized] public bool isDoneForTheDay = false;
        [NonSerialized] public string endDayMonologue = "";
        [NonSerialized] public float dayWalkSpeed = 1.5f;
        [NonSerialized] public bool hasShownLogos;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                hasShownLogos = true;
                Destroy(gameObject);
            }
        }

        private void SwitchDay()
        {
            switch (currentDay)
            {
                case Days.Day1:
                    // Logic for Day 1
                    Debug.Log("It's Day 1!");
                    SetupDay1();
                    break;
                case Days.Day2:
                    // Logic for Day 2
                    Debug.Log("It's Day 2!");
                    SetupDay2();
                    break;
                case Days.Day3:
                    // Logic for Day 3
                    Debug.Log("It's Day 3!");
                    SetupDay3();
                    break;
                case Days.Day4:
                    // Logic for Day 4
                    Debug.Log("It's Day 4!");
                    SetupDay4();
                    break;
                case Days.Day5:
                    // Logic for Day 5
                    Debug.Log("It's Day 5!");
                    SetupDay5();
                    break;
                case Days.Day6:
                    // Logic for Day 6
                    Debug.Log("It's Day 6!");
                    SetupDay6();
                    break;
                case Days.Day7:
                    // Logic for Day 7
                    Debug.Log("It's Day 7!");
                    SetupDay7();
                    break;
                default:
                    Debug.Log("Unknown day!");
                    break;
            }
        }
        private void SetupDay1()
        {
            dayWalkSpeed = 1.2f;
            wakeUpMonologue = "The medicine is working! I can walk again!\nThis... this is amazing. I really didn't think it would have nearly this much effect.";
            monolougeTimer = 3f;
            endDayMonologue = "Being able to walk has exhausted me... Hopefully that gets better tomorrow.";
            nextDay = Days.Day2;
            BaseStartOfDay("Day1");
        }
        private void SetupDay2()
        {
            dayWalkSpeed = 1.8f;
            wakeUpMonologue = "I think I feel even more nimble today.\nI should call the doctor to tell him about the results.";
            monolougeTimer = 5f;
            endDayMonologue = "Took forever to read Dr. Raphael's handwriting... My mind's all worn out. Gotta sleep.";
            nextDay = Days.Day4; // Day 3 is skipped.
            BaseStartOfDay("Day2");
        }
        private void SetupDay3()
        {
            dayWalkSpeed = 2f;
            wakeUpMonologue = "Even though I can walk, my legs are really weak after this long...\nGuess I shouldn't have quit the neuro rehab.";
            monolougeTimer = 2f;
            endDayMonologue = "Bloody intruders... The mess they made... I'm dead tired.";
            nextDay = Days.Day4;
            BaseStartOfDay("Day3");
        }
        private void SetupDay4()
        {
            dayWalkSpeed = 1.5f;
            wakeUpMonologue = "Ugh, the power's out...\nWell... I think my flashlight is on the dresser in here.\nI'll get it and check the fusebox in the hallway next.";
            monolougeTimer = 2f;
            endDayMonologue = "All that rummaging in the dark... I'm done for today.";
            nextDay = Days.Day6; // Day 5 is skipped.
            BaseStartOfDay("Day4");
        }
        private void SetupDay5()
        {
            // This day is skipped.
            dayWalkSpeed = 0.7f;
            wakeUpMonologue = "What? No! My legs...!? They're... not responding!";
            monolougeTimer = 2f;
            endDayMonologue = "";
            nextDay = Days.Day6;
            BaseStartOfDay("Day5");
        }
        private void SetupDay6()
        {
            dayWalkSpeed = 3.5f;
            wakeUpMonologue = "Oh my god... What do they want!?";
            monolougeTimer = 2f;
            endDayMonologue = "Help! Someone, please...! Help...";
            nextDay = Days.Day7;
            BaseStartOfDay("Day6");
        }
        private void SetupDay7()
        {
            dayWalkSpeed = 0f;
            wakeUpMonologue = "Back to being paralyzed...\nShould I be happy to be alive?";
            monolougeTimer = 2f;
            endDayMonologue = "";
            nextDay = Days.Day1;
            BaseStartOfDay("Day7");
        }
        private void BaseStartOfDay(string dayName)
        {
            isDoneForTheDay = false;
            SceneManager.LoadScene(dayName);
            AudioManager.StartLoopingSound(SoundType.APARTMENTBUZZING, AudioManager.instance.audSrc_ApartmentNoise);
        }
        private void CleanupDay()
        {
            wakeUpMonologue = "";
            monolougeTimer = 0;
        }
        public void StartingNewDay(Days newDay)
        {
            CleanupDay();
            currentDay = newDay;
            SwitchDay();
        }
        public IEnumerator WaitAndStartNewDay(Days newDay, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            StartingNewDay(newDay);
        }
        public string GetCurrentDayString()
        {
            string dayString = "";
            switch (currentDay)
            {
                case Days.Day1:
                    dayString = "day one";
                    break;
                case Days.Day2:
                    dayString = "day two";
                    break;
                case Days.Day3:
                    dayString = "day three";
                    break;
                case Days.Day4:
                    dayString = "day three";
                    break;
                case Days.Day5:
                    dayString = "day four";
                    break;
                case Days.Day6:
                    dayString = "day four";
                    break;
                case Days.Day7:
                    dayString = "day five";
                    break;
                default:
                    dayString = "unknown day";
                    break;
            }
            return dayString;
        }
    }
}