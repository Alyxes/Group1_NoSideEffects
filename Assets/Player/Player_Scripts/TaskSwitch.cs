using NoSideEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskSwitch : MonoBehaviour
{
    public List<ChildTrigger> childTriggers;
    public PlayerInventory currentHeldItem;
    public bool InTaskZone = false;
    private InputAction interactButton;
    private string itemIDValue;

    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
    }

    public void PlayerEntered(string triggerName)
    {
        Debug.Log("Player entered trigger: " + triggerName);
        switch (triggerName)
        {
            case "Sink":
                Debug.Log("Do action for Trigger 1");
                break;
            case "Plant":
                Debug.Log("Do action for Trigger 2");
                break;
            default:
                Debug.Log("Unknown trigger");
                break;
        }
    }

    public void PlayerExited(string triggerName)
    {
        Debug.Log("Player exited trigger: " + triggerName);
    }

    void Start()
    {
        if (currentHeldItem != null)
        {
            itemIDValue = currentHeldItem.HeldItemID;
            Debug.Log("Got itemID: " + itemIDValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTaskZone = true;
            Debug.Log("Player entered task zone. Press " + interactButton.WasPressedThisFrame() + " to use the " + itemIDValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTaskZone = false;
            Debug.Log("Player exited task zone.");
        }
    }

    private void Update()
    {
        if (InTaskZone && interactButton.WasPressedThisFrame())
        {
            Tasks();
        }
    }

    private void WaterTask()
    {
        if (InTaskZone && interactButton.WasPressedThisFrame())
        {
            transform.Find("Sink").gameObject.SetActive(true);
            Debug.Log("Filling the " + itemIDValue);
        }
    }

    public void Tasks()
    {
        switch (itemIDValue)
        {
            case "WaterCan":
                Debug.Log("I need to water the plants");
                WaterTask();
                Debug.Log("Find water");
                break;
            case "item2":
                Debug.Log("Using Screwdriver to fix screws.");
                break;
        }
    }

    // <-- New method to activate/deactivate any trigger by name
    public void SetTriggerActiveByName(string triggerName, bool isActive)
    {
        foreach (var child in childTriggers)
        {
            if (child.triggerName == triggerName)
            {
                child.SetTriggerActive(isActive);
                Debug.Log($"Trigger {triggerName} is now {(isActive ? "active" : "inactive")}");
                return;
            }
        }
        Debug.LogWarning($"Trigger {triggerName} not found!");
    }
}
