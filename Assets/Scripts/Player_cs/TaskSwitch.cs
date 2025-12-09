
using NoSideEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace NoSideEffects
{
    public class TaskSwitch : MonoBehaviour
    {
        [Header("Child Triggers")]
        public List<ChildTrigger> childTriggers;

        [Header("Player Inventory")]
        public PlayerInventory playerInventory; // Assign in inspector (optional, mostly for HeldItemID property)
        public bool InTaskZone = false;
        public static bool ItemTaskDone = false;
        public static bool Item2TaskDone = false;
        public static bool Item3TaskDone = false;
        public MeshChange meshChange;

        private InputAction interactButton;
        private string itemIDValue;
        private string currentTrigger = "";
        private bool fuse1Collected, fuse2Collected = false;



        private enum TaskState
        {
            None,
            StepOne,
            StepTwo,
            StepThree,
            StepFour,
            StepFive,
            Done
        }

        private TaskState taskState = TaskState.None;

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
        }

        private void Start()
        {
            DisableAllTriggers();
        }

        private void Update()
        {
            if (!InTaskZone) return;

            if (interactButton.WasPressedThisFrame())
                Tasks();
        }

        public void PlayerEntered(string triggerName)
        {
            currentTrigger = triggerName;
            InTaskZone = true;
        }

        public void PlayerExited(string triggerName)
        {
            if (currentTrigger == triggerName)
                currentTrigger = "";

            InTaskZone = false;
        }
        private void ActivateTrigger(string triggerName)
        {
            foreach (var child in childTriggers)
            {
                if (child.triggerName == triggerName)
                {
                    child.SetTriggerActive(true);
                    return;
                }
            }
        }

        private void DeactivateTrigger(string triggerName)
        {
            foreach (var child in childTriggers)
            {
                if (child.triggerName == triggerName)
                {
                    child.SetTriggerActive(false);
                    return;
                }
            }
        }

        public void DisableAllTriggers()
        {
            foreach (var child in childTriggers)
                child.SetTriggerActive(false);
        }

        // -----------------------------
        // Start task when an item is picked up
        // -----------------------------
        public void StartTaskForCurrentItem()
        {
            // Use static variable directly
            if (PlayerInventory.currentHeldItem == null) return;

            // Get item ID
            itemIDValue = PlayerInventory.currentHeldItem.itemID;

            switch (itemIDValue)
            {
                case "WaterCan":
                    Debug.Log("Starting Water Task automatically");
                    if (taskState != TaskState.Done)
                        taskState = TaskState.None;

                    WaterTask(); // Activate Sink trigger immediately
                    break;

                case "ToolBox":
                    Debug.Log("Starting ToolBox Task automatically");
                    if (taskState != TaskState.Done)
                        taskState = TaskState.None;
                    Item2Task(); // Activate TriggerA immediately
                    break;
                case "Key":
                        Debug.Log("Starting Key Task automatically");
                        if (taskState != TaskState.Done)
                            taskState = TaskState.None;
                        Item3Task(); // Activate TriggerA immediately
                        break;

                default:
                    Debug.Log("No task assigned for this item");
                    break;
            }
        }

        public void Tasks()
        {
            if (PlayerInventory.currentHeldItem != null)
                itemIDValue = PlayerInventory.currentHeldItem.itemID;

            switch (itemIDValue)
            {
                case "WaterCan":
                    if (ItemTaskDone)
                    {
                        Debug.Log("Water Task already completed.");
                        return;
                    }
                    WaterTask();
                    break;

                case "ToolBox":
                    if (Item2TaskDone)
                    {
                        Debug.Log("ToolBox Task already completed.");
                        return;
                    }
                    Item2Task();
                    break;
                    case "Key":
                    if (Item3TaskDone)
                    {
                        Debug.Log("Key Task already completed.");
                        return;
                    }
                    Item3Task();
                    break;

            }
        }

        private void WaterTask()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Sink");
                    taskState = TaskState.StepOne;
                    Debug.Log("Go to the Sink to start filling water.");
                    HUD.instance.SetSubTitleText("Let's give the plants some water.\nGotta fill this up in the kitchen.");
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "Sink")
                    {
                        Debug.Log("You must be at the Sink to fill water.");
                        return;
                    }

                    Debug.Log("Filling watering can...");
                    DeactivateTrigger("Sink");
                    ActivateTrigger("Plant");
                    HUD.instance.SetSubTitleText("Some nice water for my little planties...");
                    taskState = TaskState.StepTwo;
                    break;

                case TaskState.StepTwo:
                    if (currentTrigger != "Plant")
                    {
                        Debug.Log("Go to the Plant to water it.");
                        return;
                    }

                    Debug.Log("Watering the plant...");
                    DeactivateTrigger("Plant");
                    ActivateTrigger("Sink");
                    HUD.instance.SetSubTitleText("Oh, water's out. They'll need a little more...\nLet's fill this up again.");
                    taskState = TaskState.StepThree;
                    Debug.Log("I need more water");
                    break;
                case TaskState.StepThree:
                    if (currentTrigger != "Sink")
                    {
                        Debug.Log("You must be at the Sink to fill water.");
                        return;
                    }

                    Debug.Log("Filling watering can...");
                    DeactivateTrigger("Sink");
                    ActivateTrigger("Plant");
                    meshChange.DeactivateChild("plant_alive");
                    meshChange.ActivateChild("plant_dead");
                    HUD.instance.SetSubTitleText("Theeere we go.");
                    taskState = TaskState.StepFour;
                    break;
                case TaskState.StepFour:
                    if (currentTrigger != "Plant")
                    {
                        Debug.Log("Go to the Plant to water it.");
                        return;
                    }

                    Debug.Log("Watering the plant... Task Complete!");
                    HUD.instance.SetSubTitleText("...Are you finally giving up on me as well?\nI guess I don't deserve any living company...");
                    DeactivateTrigger("Plant");
                    taskState = TaskState.Done;
                    StartCoroutine(DSM.instance.WaitAndStartNewDay(DSM.Days.Day3, 10f));
                    break;
                case TaskState.Done:
                    ItemTaskDone = true;
                    Debug.Log("Task already completed.");
                    break;
            }
        }
        private void Item2Task()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("FuseBox");
                    taskState = TaskState.StepOne;
                    Debug.Log("Go to Fusebox to start ToolBox task.");
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "FuseBox")
                    {
                        Debug.Log("You must be at the FuseBox to proceed.");
                        return;
                    }
                    Debug.Log("I need to find the fuses...");
                    DeactivateTrigger("FuseBox");
                    ActivateTrigger("Fuse1");
                    ActivateTrigger("Fuse2");
                    taskState = TaskState.StepTwo;
                    break;
                case TaskState.StepTwo:
                    if (currentTrigger != "Fuse1" && currentTrigger != "Fuse2")
                    {
                        Debug.Log("Go to one of the fuses to pick it up.");
                        return;
                    }

                    Debug.Log("Picking up fuse...");

                    if (currentTrigger == "Fuse1")
                    {
                        fuse1Collected = true;
                        DeactivateTrigger("Fuse1");
                        Transform child = transform.Find("Fuse1");
                        if (child != null)
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    else if (currentTrigger == "Fuse2")
                    {
                        fuse2Collected = true;
                        DeactivateTrigger("Fuse2");
                        Transform child = transform.Find("Fuse2");
                        if (child != null)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    // Only proceed when both are collected
                    if (fuse1Collected && fuse2Collected)
                    {
                        ActivateTrigger("FuseBox");
                        taskState = TaskState.StepThree;
                        HUD.instance.SetSubTitleText("Got the fuses. Let's put them back in the box.");
                    }
                    else
                    {
                        HUD.instance.SetSubTitleText("You picked up a fuse. Find the other one.");
                    }
                    break;
                    case TaskState.StepThree:
                    if (currentTrigger != "FuseBox")
                    {
                        Debug.Log("You must be at the FuseBox to proceed.");
                        return;
                    }
                    Debug.Log("Inserting fuse into FuseBox... Task Complete!");
                    DeactivateTrigger("FuseBox");
                    taskState = TaskState.Done;
                    Item2TaskDone = true;
                    HUD.instance.SetSubTitleText("The power's back on! Finally, some light in this gloomy place.");
                    break;
                    case TaskState.Done:
                    Debug.Log("Task already completed.");
                    break;
            }
        }
        private void Item3Task()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Door");
                    taskState = TaskState.StepOne;
                    Debug.Log("Go to the Door to use the Key.");
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "Door")
                    {
                        Debug.Log("You must be at the Door to proceed.");
                        return;
                    }
                    Debug.Log("I need to remove the eyes");
                    DeactivateTrigger("Door");
                    ActivateTrigger("Eye1");

                    taskState = TaskState.StepTwo;
                    break;
                case TaskState.StepTwo:
                    if (currentTrigger != "Eye1")
                    {
                        Debug.Log("You must be at the Eye to proceed.");
                        return;
                    }
                    Debug.Log("Removing eye... Task Complete!");
                    DeactivateTrigger("Eye1");
                    Transform child = transform.Find("Eye1");
                    if (child != null)
                    {
                        Destroy(child.gameObject);
                    }
                    ActivateTrigger("Eye2");
                    taskState = TaskState.StepThree;
                    break;
                case TaskState.StepThree:
                    if (currentTrigger != "Eye2")
                    {
                        Debug.Log("You must be at the Eye to proceed.");
                        return;
                    }
                    Debug.Log("Removing eye... Task Complete!");
                    DeactivateTrigger("Eye2");
                    Transform child2 = transform.Find("Eye2");
                    if (child2 != null)
                    {
                        Destroy(child2.gameObject);
                    }
                    ActivateTrigger("Eye3");
                    ActivateTrigger("eye4");
                    taskState = TaskState.StepFour;
                    break;
                    case TaskState.StepFour:
                    if (currentTrigger != "Eye3" && currentTrigger != "Eye4")
                    {
                        Debug.Log("Go to one of the eyes and destroy it");
                        return;
                    }

                    Debug.Log("Destroying Eye");

                    if (currentTrigger == "Eye3")
                    {
                        fuse1Collected = true;
                        DeactivateTrigger("Eye3");
                        Transform child3 = transform.Find("Eye3");
                        if (child3 != null)
                        {
                            Destroy(child3.gameObject);
                        }
                    }
                    else if (currentTrigger == "Eye4")
                    {
                        fuse2Collected = true;
                        DeactivateTrigger("Eye4");
                        Transform child4 = transform.Find("Eye4");
                        if (child4 != null)
                        {
                            Destroy(child4.gameObject);
                        }
                    }

                    // Only proceed when both are collected
                    if (fuse1Collected && fuse2Collected)
                    {
                        ActivateTrigger("Door");
                        taskState = TaskState.StepFive;
                        HUD.instance.SetSubTitleText("Alright let's try the door again");
                    }
                    else
                    {
                        HUD.instance.SetSubTitleText("You got more eyes to destroy");
                    }
                    break;
                    case TaskState.StepFive:
                    Item3TaskDone = true;
                    HUD.instance.SetSubTitleText("The door creaks open, revealing a path to freedom.\nMaybe there's hope after all.");
                    // Proceed to next scene or day
                    StartCoroutine(DSM.instance.WaitAndStartNewDay(DSM.Days.Day4, 10f));
                    break;
                case TaskState.Done:
                    Debug.Log("Task already completed.");
                    break;

            }
        }
        
    }
}
