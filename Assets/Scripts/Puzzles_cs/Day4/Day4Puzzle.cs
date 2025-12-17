using JetBrains.Annotations;
using NoSideEffects;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Day4Puzzle : MonoBehaviour
{
    public Lightswitch lightswitch; 
    private bool inZone = false;
    public TaskSwitch day4TaskSwitch;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnter(other);
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }
    private void Update()
    {
        if (inZone)
        {
            day4TaskSwitch.Day4Task();
        }
    }
}
   