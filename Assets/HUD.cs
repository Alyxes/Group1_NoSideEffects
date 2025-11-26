using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    private void awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
