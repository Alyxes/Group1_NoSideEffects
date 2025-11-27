using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoSideEffects
{
    public class DayStateMachine : MonoBehaviour
    {
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
        [NonSerialized] public Days currentDay = Days.Day1;
        
        public void GameStateMachine()
        {
            switch(currentDay)
                {
                    case Days.Day1:
                        // Logic for Day 1
                        Debug.Log("It's Day 1!");
                        SetupDay1();
                        EndingDay1();
                        CleanupDay();
                        break;
                    case Days.Day2:
                        // Logic for Day 2
                        Debug.Log("It's Day 2!");
                        SetupDay2();
                        EndingDay2();
                        CleanupDay();
                    break;
                    case Days.Day3:
                        // Logic for Day 3
                        Debug.Log("It's Day 3!");
                        SetupDay3();
                        EndingDay3();
                        CleanupDay();
                    break;
                    case Days.Day4:
                        // Logic for Day 4
                        Debug.Log("It's Day 4!");
                        SetupDay4();
                        EndingDay4();
                        CleanupDay();
                    break;
                    case Days.Day5:
                        // Logic for Day 5
                        Debug.Log("It's Day 5!");
                        SetupDay5();
                        EndingDay5();
                        CleanupDay();
                    break;
                    case Days.Day6:
                        // Logic for Day 6
                        Debug.Log("It's Day 6!");
                        SetupDay6();
                        EndingDay6();
                        CleanupDay();
                    break;
                    case Days.Day7:
                        // Logic for Day 7
                        Debug.Log("It's Day 7!");
                        SetupDay7();
                        EndingDay7();
                        CleanupDay();
                    break;
                    default:
                        Debug.Log("Unknown day!");
                        break;
            }
        }
        private void SetupDay1()
        {
            SceneManager.LoadScene("Day1");

        }
        private void SetupDay2()
        {
            SceneManager.LoadScene("Day2");
        }
        private void SetupDay3()
        {
            SceneManager.LoadScene("Day3");
        }
        private void SetupDay4()
        {
            SceneManager.LoadScene("Day4");
        }
        private void SetupDay5()
        {
            SceneManager.LoadScene("Day5");
        }
        private void SetupDay6()
        {
            SceneManager.LoadScene("Day6");
        }
        private void SetupDay7()
        {
            SceneManager.LoadScene("Day7");
        }
        private void CleanupDay()
        {
            
        }
        // These days does not use currentDay++ yet, because we might need to cut some days, and then day 3 might jump to day 5 directly.
        private void EndingDay1()
        {
            currentDay = Days.Day2;
        }
        private void EndingDay2()
        {
            currentDay = Days.Day3;
        }
        private void EndingDay3()
        {
            currentDay = Days.Day4;
        }
        private void EndingDay4()
        {
            currentDay = Days.Day5;
        }
        private void EndingDay5()
        {
            currentDay = Days.Day6;
        }
        private void EndingDay6()
        {
            currentDay = Days.Day7;
        }
        private void EndingDay7()
        {
            
        }
    }
}