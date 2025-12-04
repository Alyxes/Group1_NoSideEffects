using System;
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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
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
            BaseStartOfDay("Day1");
        }
        private void SetupDay2()
        {
            BaseStartOfDay("Day2");
        }
        private void SetupDay3()
        {
            BaseStartOfDay("Day3");
        }
        private void SetupDay4()
        {
            BaseStartOfDay("Day4");
        }
        private void SetupDay5()
        {
            BaseStartOfDay("Day5");
        }
        private void SetupDay6()
        {
            BaseStartOfDay("Day6");
        }
        private void SetupDay7()
        {
            BaseStartOfDay("Day7");
        }
        private void BaseStartOfDay(string dayName)
        {
            SceneManager.LoadScene(dayName);
            AudioManager.StartLoopingSound(SoundType.APARTMENTBUZZING);
        }
        private void CleanupDay()
        {
            // Possible code for cleaning up the day before starting a new one. Not sure this will be needed.
        }
        public void StartingNewDay(Days newDay)
        {
            CleanupDay();
            currentDay = newDay;
            SwitchDay();
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
                    dayString = "day four";
                    break;
                case Days.Day5:
                    dayString = "day five";
                    break;
                case Days.Day6:
                    dayString = "day six";
                    break;
                case Days.Day7:
                    dayString = "day seven";
                    break;
                default:
                    dayString = "unknown day";
                    break;
            }
            return dayString;
        }
    }
}