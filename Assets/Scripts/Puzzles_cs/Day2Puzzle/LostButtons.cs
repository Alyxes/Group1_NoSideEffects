using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LostButtons : MonoBehaviour
{
    private bool inZone = false;
    private InputAction interactButton;

    // Static list to track picked-up objects
    public static List<string> pickedUpObjects = new List<string>();

    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }

    private void Update()
    {
        if (inZone && interactButton.WasPressedThisFrame())
        {
            Debug.Log(gameObject.name + " picked up!");

            // Add this object to the static list
            pickedUpObjects.Add(gameObject.name);

            Destroy(gameObject);
        }
    }
}

