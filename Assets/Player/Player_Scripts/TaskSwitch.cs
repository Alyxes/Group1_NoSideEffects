using NoSideEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TaskSwitch : MonoBehaviour
{
    public ItemPickup currentHeldItem;
    public PlayerInventory playerInventory;
    bool InTaskZone = false;
    private InputAction interactButton;
    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTaskZone = true;
            Debug.Log("Player entered task zone. Press " + interactButton.WasPressedThisFrame() + " to use the " + currentHeldItem.itemID);
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
            switch(currentHeldItem.itemID) 
                {
                case "WateringCan":
                    WaterTask();
                    break;
                case "Screwdriver":
                    Debug.Log("Using Screwdriver to fix screws.");
                    break;
            }
        }
    }
    private void WaterTask()
    {
        if(InTaskZone && interactButton.WasPressedThisFrame())
        {
             Debug.Log("Filling the " + currentHeldItem.itemID);

        }
    }

    TaskSwitch()
    {
        switch(currentHeldItem.itemID)
            {
            case "Item":
                Debug.Log("I need to water the plants");

                transform.Find("Sink").gameObject.SetActive(true);
                Debug.Log("Find water");
                

                break;
            case "item2":
                Debug.Log("Using Screwdriver to fix screws.");
                break;
        }
    }
}
