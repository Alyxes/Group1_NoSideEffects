using NoSideEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class TaskSwitch : MonoBehaviour
{
    public PlayerInventory currentHeldItem;
    bool InTaskZone = false;
    private InputAction interactButton;
    private string itemIDValue;
    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
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
        if(InTaskZone && interactButton.WasPressedThisFrame())
        {
             Debug.Log("Filling the " + itemIDValue);

        }
    }

    public void Tasks()
    {
        switch(itemIDValue)
            {
            case "WaterCan":
                Debug.Log("I need to water the plants");
                WaterTask();

                transform.Find("Sink").gameObject.SetActive(true);
                Debug.Log("Find water");
                

                break;
            case "item2":
                Debug.Log("Using Screwdriver to fix screws.");
                break;
        }
    }
}