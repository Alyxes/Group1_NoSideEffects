using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoSideEffects
{
    public class TaskSwitch : MonoBehaviour
    {
        [Header("Child Triggers")]
        public List<ChildTrigger> childTriggers;

        [Header("Mesh Change")]
        public List<MeshChange> meshChanges;

        public Lightswitch lightSwitch;
        public Door door;
        [Header("Player Inventory")]
        public PlayerInventory playerInventory;
        public GameObject keyZone;

        public bool InTaskZone = false;
        public static bool ItemTaskDone = false;
        public static bool Item2TaskDone = false;
        public static bool Item3TaskDone = false;

        private InputAction interactButton;
        private string currentTrigger = "";
        private bool fuse1Collected, fuse2Collected = false;

        private enum TaskState { None, StepOne, StepTwo, StepThree, StepFour, Done }
        private TaskState taskState = TaskState.None;

        [System.Obsolete]
        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");

            // Ensure playerInventory is assigned
            if (playerInventory == null)
                playerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Start()
        {
            var keyZone = transform.Find("KeyZone");
            if (keyZone != null) keyZone.gameObject.SetActive(false);

            DisableAllTriggers();
            if (SceneManager.GetActiveScene().name == "Day4")
            {
                lightSwitch.ToggleLights();
                lightSwitch.ChangeMaterials();
                Day4Task();
            }
        }

        private void Update()
        {
            if (!InTaskZone) return;

            if (interactButton.WasPressedThisFrame())
                Tasks();
        }
        private void ActivateMesh(string childName)
        {
            foreach (var mc in meshChanges)
            {
                mc.ActivateChild(childName);
            }
        }

        private void DeactivateMesh(string childName)
        {
            foreach (var mc in meshChanges)
            {
                mc.DeactivateChild(childName);
            }
        }

        public void PlayerEntered(string triggerName)
        {
            currentTrigger = triggerName;
            InTaskZone = true;
        }

        public void PlayerExited(string triggerName)
        {
            if (currentTrigger == triggerName) currentTrigger = "";
            InTaskZone = false;
        }

        public void DisableAllTriggers()
        {
            foreach (var child in childTriggers)
                child.SetTriggerActive(false);
        }

        private void ActivateTrigger(string triggerName)
        {
            foreach (var child in childTriggers)
                if (child.triggerName == triggerName)
                    child.SetTriggerActive(true);
        }

        private void DeactivateTrigger(string triggerName)
        {
            foreach (var child in childTriggers)
                if (child.triggerName == triggerName)
                    child.SetTriggerActive(false);
        }

        // -----------------------------
        // Start task for current item (checks both hands)
        // -----------------------------
        public void StartTaskForCurrentItem()
        {
            ItemPickup itemToUse = PlayerInventory.currentHeldItem ?? PlayerInventory.currentHeldItemLeft;

            if (itemToUse == null)
            {
                Debug.Log("No item in either hand to start task.");
                return;
            }

            Debug.Log("Starting task for: " + itemToUse.itemID);
            StartTaskByID(itemToUse.itemID);
        }

        public void Tasks()
        {
            ItemPickup itemToUse = PlayerInventory.currentHeldItem ?? PlayerInventory.currentHeldItemLeft;

            if (itemToUse == null)
            {
                Debug.Log("No item in either hand to use task.");
                return;
            }

            StartTaskByID(itemToUse.itemID);
        }

        private void StartTaskByID(string itemID)
        {
            switch (itemID)
            {
                case "WaterCan":
                    if (!ItemTaskDone) Day1Task();
                    break;
                case "Knife":
                    if (!Item3TaskDone) Day6Task();
                    break;
                case "Key":
                    if (!Item3TaskDone) KeyTask();
                    break;
                //case "Flashlight":
                  //  if (!Item2TaskDone) Day4Task();
                    //break;
                default:
                    Debug.Log("No task assigned for this item: " + itemID);
                    break;
            }
        }

private void Day1Task()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Sink");
                    Debug.Log("Go to the Sink to start filling water.");
                    HUD.instance.SetSubTitleText("Let's give the plants some water.\nGotta fill this up in the kitchen.");
                    taskState = TaskState.StepOne;
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
                    DeactivateMesh("HealthyPlant_SM (1)"); DeactivateMesh("HealthyPlant_SM (2)"); DeactivateMesh("HealthyPlant_SM (3)");
                    ActivateMesh("DeadPlant_SM (1)"); ActivateMesh("DeadPlant_SM (2)"); ActivateMesh("DeadPlant_SM (3)");
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
                    DSM.instance.isDoneForTheDay = true;
                    break;
            }
        }
        public void Day4Task()
        {
            if (Item2TaskDone) return;

            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("FuseBox");
                    taskState = TaskState.StepOne;
                    Debug.Log("Go to Fusebox to start task.");
                    HUD.instance.SetSubTitleText("Great, the power's out now too...\nI need to find and change the fuses.");
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
                        HUD.instance.SetSubTitleText("Got one fuse. Find the other one.");
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
                    lightSwitch.ToggleLights();
                    lightSwitch.ResetMaterials();
                    Item2TaskDone = true;
                    HUD.instance.SetSubTitleText("The power's back on! Finally, some light in this gloomy place.");
                    DSM.instance.isDoneForTheDay = true;
                    break;
            }
        }
        private void Day6Task()
        {
            switch (taskState)
            {
                case TaskState.None:
                    ActivateTrigger("Eye1");
                    HUD.instance.SetSubTitleText("Woahh... What are those things?\n" +
                        "They look like eyes... I need to get rid of them.");
                    taskState = TaskState.StepOne;
                    break;
                case TaskState.StepOne:
                    if (currentTrigger != "Eye1")
                    {
                        Debug.Log("You must be at the Door to proceed.");
                        return;
                    }
                    HUD.instance.SetSubTitleText("Bye bye mister eye.");
                    DeactivateTrigger("Eye1");
                    DeactivateMesh("Eye1"); ActivateMesh("Eye1Dmg");
                    ActivateTrigger("Eye2");
                    taskState = TaskState.StepTwo;
                    break;
                case TaskState.StepTwo:
                    if (currentTrigger != "Eye2")
                    {
                        Debug.Log("You must be at the Eye to proceed.");
                        return;
                    }
                    HUD.instance.SetSubTitleText("Another one bites the dust.");
                    DeactivateTrigger("Eye2");
                    DeactivateMesh("Eye2"); ActivateMesh("Eye2Dmg");
                    ActivateTrigger("Eye3");
                    ActivateTrigger("Eye4");
                    taskState = TaskState.StepThree;
                    break;

                case TaskState.StepThree:
                    HUD.instance.SetSubTitleText("More!?!...Where are they coming from!?");
                    if (currentTrigger != "Eye3" && currentTrigger != "Eye4")
                    {
                        Debug.Log("Go to one of the eyes and destroy it");
                        return;
                    }

                    HUD.instance.SetSubTitleText("Why are they blocking the windows?");

                    if (currentTrigger == "Eye3")
                    {
                        fuse1Collected = true;
                        DeactivateTrigger("Eye3");
                        DeactivateMesh("Eye3"); ActivateMesh("Eye3Dmg");
                    }
                    else if (currentTrigger == "Eye4")
                    {
                        fuse2Collected = true;
                        DeactivateTrigger("Eye4");
                        DeactivateMesh("Eye4"); ActivateMesh("Eye4Dmg");
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
                        HUD.instance.SetSubTitleText("I gotta get the last one.");
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
                    transform.Find("EyeTrigger5").gameObject.SetActive(false);
                    keyZone.SetActive(true);
                    taskState = TaskState.Done;
                    HUD.instance.SetSubTitleText("All eyes are gone. And... \n" + "it dropped a mysterious key?");
                    break;

                case TaskState.Done:
                    Item3TaskDone = true;
                    break;

            }
        }
        public void KeyTask()
        {
            ActivateTrigger("Door");
            door.canBeOpened = true;
            Debug.Log("Go to the door to use the key.");
            HUD.instance.SetSubTitleText("This key looks like it fits the door.\nLet's see what's in here.");
            DSM.instance.isDoneForTheDay = true;
        }
    }
}
