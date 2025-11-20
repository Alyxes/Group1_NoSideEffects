using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] public bool isCollected = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            Debug.Log("Item picked up!");
            Destroy(gameObject);
        }
    }

}
