using NoSideEffects;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : MonoBehaviour
{
    public MeshChange MeshChanger;
    public List<Light> lights;
    private bool isOn = false; // lights start OFF


    public void ToggleLights()
    {
        isOn = !isOn;

        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = isOn;
        }
    }
    public void SetLights(bool on)
    {
        isOn = on;
        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = isOn;
        }
    }

    public void TurnOffLights() => SetLights(false);
    public void TurnOnLights() => SetLights(true);

    public void ToggleMesh()
    {

    }

}

