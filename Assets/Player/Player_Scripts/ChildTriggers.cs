using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    public TaskSwitch parentTrigger;  // Assign parent in Inspector
    public string triggerName;         // Set the trigger name in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentTrigger.InTaskZone = true;
            parentTrigger.PlayerEntered(triggerName); // Send the name to parent
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentTrigger.InTaskZone = false;
            parentTrigger.PlayerExited(triggerName); // Send the name to parent
        }
    }

    // <-- New method to enable/disable the trigger
    public void SetTriggerActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
