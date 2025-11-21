using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public bool isCollected = false;
    public bool inZone = false;
    public GameObject HoldPoint;
    InputAction interactButton;

    private void Start()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
        HoldPoint.SetActive(false);
    }

    private void Update()
    {
        if (inZone && interactButton.WasPressedThisFrame())
        {
            if (!isCollected)
            {
                PickUpItem();
            }
            else
            {
                DropItem();
            }
        }
    }

    private void PickUpItem()
    {
        if (GameObject.FindWithTag("Item1") == null)
        {
            isCollected = true;
            transform.GetChild(0).gameObject.SetActive(false); // hide the item model
            HoldPoint.SetActive(true); // show it at the hold point
            gameObject.tag = "Item1";
            Debug.Log("Item picked up");
        }
        else
        {
            Debug.Log("You are already holding an item!");
        }
    }

    private void DropItem()
    {
        isCollected = false;
        transform.GetChild(0).gameObject.SetActive(true); // show the item model again
        HoldPoint.SetActive(false);
        gameObject.tag = "Untagged";
        Debug.Log("Item dropped");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            Debug.Log("Press E to pick up or drop item");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            Debug.Log("Left pickup zone");
        }
    }
}

