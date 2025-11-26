using System;
using UnityEngine;

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
                        CleanupDay();
                        break;
                    case Days.Day2:
                        // Logic for Day 2
                        Debug.Log("It's Day 2!");
                        break;
                    case Days.Day3:
                        // Logic for Day 3
                        Debug.Log("It's Day 3!");
                        break;
                    case Days.Day4:
                        // Logic for Day 4
                        Debug.Log("It's Day 4!");
                        break;
                    case Days.Day5:
                        // Logic for Day 5
                        Debug.Log("It's Day 5!");
                        break;
                    case Days.Day6:
                        // Logic for Day 6
                        Debug.Log("It's Day 6!");
                        break;
                    case Days.Day7:
                        // Logic for Day 7
                        Debug.Log("It's Day 7!");
                        break;
                    default:
                        Debug.Log("Unknown day!");
                        break;
            }
        }
        private void SetupDay1()
        {
            
        }
        private void SetupDay2()
        {

        }
        private void SetupDay3()
        {

        }
        private void SetupDay4()
        {

        }
        private void SetupDay5()
        {

        }
        private void SetupDay6()
        {

        }
        private void SetupDay7()
        {

        }
        private void CleanupDay()
        {
            
        }
    }
}