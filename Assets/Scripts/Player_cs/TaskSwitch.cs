
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
        [Header("Mesh Change")]
        public List<MeshChange> meshNamesToChange;

        public MeshChange meshChange;
        [Header("Player Inventory")]
        public PlayerInventory playerInventory; // Assign in inspector (optional, mostly for HeldItemID property)
        public bool InTaskZone = false;
        public static bool ItemTaskDone = false;
        public static bool Item2TaskDone = false;
        public static bool Item3TaskDone = false;


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
    var keyZone = transform.Find("KeyZone");
    if (keyZone != null)
        keyZone.gameObject.SetActive(false);

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

                    Day1Task(); // Activate Sink trigger immediately
                    break;

                case "ToolBox":
                    Debug.Log("Starting ToolBox Task automatically");
                    if (taskState != TaskState.Done)
                        taskState = TaskState.None;
                    Day4Task(); // Activate TriggerA immediately
                    break;

                case "Knife":
                    Debug.Log("Starting Knife Task automatically");
                    if (taskState != TaskState.Done)
                        taskState = TaskState.None;
                    Day6Task(); // Activate TriggerA immediately
                    break;
                case "Key":
                    Debug.Log("Starting Knife Task automatically");
                    if (taskState != TaskState.Done)
                        taskState = TaskState.None;
                    KeyTask(); // Activate TriggerA immediately
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
                    Day1Task();
                    break;

                case "ToolBox":
                    if (Item2TaskDone)
                    {
                        Debug.Log("ToolBox Task already completed.");
                        return;
                    }
                    Day4Task();
                    break;

                case "Knife":

                    if (Item3TaskDone)
                    {
                        Debug.Log("Knife Task already completed.");
                        return;
                    }
                    Day6Task();
                    break;
                case "Key":
                    if (Item3TaskDone)
                    {
                        Debug.Log("Key Task already completed.");
                        return;
                    }
                    KeyTask();
                    break;

            }
        }

        private void Day1Task()
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
                    meshChange.DeactivateChild("HealthyPlant_SM (1)"); meshChange.DeactivateChild("HealthyPlant_SM (2)"); meshChange.DeactivateChild("HealthyPlant_SM (3)");
                    meshChange.ActivateChild("DeadPlant_SM (1)"); meshChange.ActivateChild("DeadPlant_SM (2)"); meshChange.ActivateChild("DeadPlant_SM (3)");
                    HUD.instance.SetSubTitleText("Theeere we go.");
                    taskState = TaskState.Done;
                    break;
                case TaskState.Done:
                    if (currentTrigger != "Plant")
                    {
                        Debug.Log("Go to the Plant to water it.");
                        return;
                    }

                    Debug.Log("Watering the plant... Task Complete!");
                    HUD.instance.SetSubTitleText("...Are you finally giving up on me as well?\nI guess I don't deserve any living company...");
                    DeactivateTrigger("Plant");
                    StartCoroutine(DSM.instance.WaitAndStartNewDay(DSM.Days.Day2, 10f));
                    break;
            }
        }
        private void Day4Task()
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
                        taskState = TaskState.Done;
                        HUD.instance.SetSubTitleText("Got the fuses. Let's put them back in the box.");
                    }
                    else
                    {
                        HUD.instance.SetSubTitleText("You picked up a fuse. Find the other one.");
                    }
                    break;

                case TaskState.Done:


                    if (currentTrigger != "FuseBox")
                    {
                        Debug.Log("You must be at the FuseBox to proceed.");
                        return;
                    }
                    Debug.Log("Inserting fuse into FuseBox... Task Complete!");
                    DeactivateTrigger("FuseBox");
                    Item2TaskDone = true;
                    HUD.instance.SetSubTitleText("The power's back on! Finally, some light in this gloomy place.");
                    break;
            }
        }
        private void Day6Task()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Eye1");
                    Debug.Log("Gotta get rid of these eyes");
                    taskState = TaskState.StepOne;
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "Eye1")
                    {
                        Debug.Log("You must be at the Door to proceed.");
                        return;
                    }
                    Debug.Log("I need to remove the eyes");
                    DeactivateTrigger("Eye1");
                    //Insert to change mesh here
                    ActivateTrigger("Eye2");
                    taskState = TaskState.StepTwo;
                    break;
                case TaskState.StepTwo:
                    if (currentTrigger != "Eye2")
                    {
                        Debug.Log("You must be at the Eye to proceed.");
                        return;
                    }
                    Debug.Log("Removing eye...");
                    DeactivateTrigger("Eye2");
                    //Insert to change mesh here
                    ActivateTrigger("Eye3");
                    ActivateTrigger("Eye4");
                    taskState = TaskState.StepThree;
                    break;

                case TaskState.StepThree:
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
                        //Insert to change mesh here
                    }
                    else if (currentTrigger == "Eye4")
                    {
                        fuse2Collected = true;
                        DeactivateTrigger("Eye4");
                        //Insert to change mesh here
                    }
                    // Only proceed when both are destroyed
                    if (fuse1Collected && fuse2Collected)
                    {
                        ActivateTrigger("Eye5");
                        taskState = TaskState.StepFour;
                        HUD.instance.SetSubTitleText("One last eye remains. Time to finish this.");
                    }
                    else
                    {
                        HUD.instance.SetSubTitleText("You got more eyes to destroy");
                    }
                    break;

                case TaskState.StepFour:
                    if (currentTrigger != "Eye5")
                    {
                        Debug.Log("You must destroy the last eye to proceed");
                        return;
                    }
                    Debug.Log("Using Knife on eye... Now to the door");
                    DeactivateTrigger("Eye5");
                    //Insert to change mesh here
                    transform.Find("KeyZone").gameObject.SetActive(true);
                    taskState = TaskState.Done;
                    HUD.instance.SetSubTitleText("All eyes are gone. Let's open this door.");
                    break;

                case TaskState.Done:
                    Item3TaskDone = true;
                    break;

            }
        }
        public void KeyTask()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Door");
                    taskState = TaskState.StepOne;
                    Debug.Log("Go to the door to use the key.");
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "Door")
                    {
                        Debug.Log("You must be at the Door to proceed.");
                        return;
                    }
                    Debug.Log("Using Key on Door... Task Complete!");
                    DeactivateTrigger("Door");
                    Item3TaskDone = true;
                    HUD.instance.SetSubTitleText("The door's finally open.");
                    break;
            }
        }
    }
}
