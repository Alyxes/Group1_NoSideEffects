using UnityEngine;
using UnityEngine.InputSystem;

public class CombinationPickUp : MonoBehaviour
{
    public bool HasCombination = false;
    private bool InZone = false;

    private InputAction interactButton;

    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
        if (interactButton == null)
            Debug.LogError("SceneChecker: Could not find 'Interact' action! Make sure your Input Actions asset is set as Default.");
    }
    private void Update()
    {
        if (interactButton == null) return;

        if (InZone && interactButton.WasPressedThisFrame())
        {
          HasCombination = true;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InZone = false;
        }
    }
}
