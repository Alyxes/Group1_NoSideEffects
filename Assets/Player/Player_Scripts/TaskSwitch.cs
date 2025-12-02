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

    private InputAction interactButton;
    private string itemIDValue;

    private string currentTrigger = "";
    public bool InTaskZone = false;

    private enum WaterState
    {
        None,
        GoToSink,
        Filled,
        GoToPlant,
        Done
    }

    private WaterState waterState = WaterState.None;

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
                waterState = WaterState.None;
                WaterTask(); // Activate Sink trigger immediately
                break;

            case "Screwdriver":
                Debug.Log("Starting Screwdriver Task automatically");
                // Add Screwdriver logic
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
                WaterTask();
                break;
        }
    }

    private void WaterTask()
    {
        switch (waterState)
        {
            case WaterState.None:
                ActivateTrigger("Sink");
                waterState = WaterState.GoToSink;
                Debug.Log("Go to the Sink to start filling water.");
                break;

            case WaterState.GoToSink:
                if (currentTrigger != "Sink")
                {
                    Debug.Log("You must be at the Sink to fill water.");
                    return;
                }

                Debug.Log("Filling watering can...");
                DeactivateTrigger("Sink");
                ActivateTrigger("Plant");
                waterState = WaterState.Filled;
                break;

            case WaterState.Filled:
                if (currentTrigger != "Plant")
                {
                    Debug.Log("Go to the Plant to water it.");
                    return;
                }

                Debug.Log("Watering the plant...");
                DeactivateTrigger("Plant");
                waterState = WaterState.Done;
                Debug.Log("Water Task Complete!");
                break;

            case WaterState.Done:
                Debug.Log("Task already completed.");
                break;
        }
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
}
