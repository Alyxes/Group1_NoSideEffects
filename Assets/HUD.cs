using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public TMP_Text PickupText;


    private void awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
