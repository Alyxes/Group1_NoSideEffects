
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NoSideEffects;

public class TaskSwitch : MonoBehaviour
{
    [Header("Child Triggers")]
    public List<ChildTrigger> childTriggers;

    [Header("Player Inventory")]
    public PlayerInventory playerInventory; // Assign in inspector (optional, mostly for HeldItemID property)
    public bool InTaskZone = false;
    public static bool ItemTaskDone = false;
    public static bool Item2TaskDone = false;
    public MeshChange meshChange;

    private InputAction interactButton;
    private string itemIDValue;
    private string currentTrigger = "";



    private enum TaskState
    {
        None,
        StepOne,
        StepTwo,
        StepThree,
        StepFour,
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
                taskState = TaskState.None;
                WaterTask(); // Activate Sink trigger immediately
                break;

            case "Item2":
                Debug.Log("Starting Screwdriver Task automatically");
                taskState = TaskState.None;
                Item2Task(); // Activate TriggerA immediately
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

            case "Item2":
                if (Item2TaskDone)
                {
                    Debug.Log("Item2 Task already completed.");
                    return;
                }
                Item2Task();
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
                taskState = TaskState.StepFour;
                break;
            case TaskState.StepFour:
                if (currentTrigger != "Plant")
                {
                    Debug.Log("Go to the Plant to water it.");
                    return;
                }

                Debug.Log("Watering the plant... Task Complete!");
                DeactivateTrigger("Plant");
                taskState = TaskState.Done;
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
                ActivateTrigger("TriggerA");
                taskState = TaskState.StepOne;
                Debug.Log("Go to TriggerA to start Item2 task.");
                break;
        }
    }
}
