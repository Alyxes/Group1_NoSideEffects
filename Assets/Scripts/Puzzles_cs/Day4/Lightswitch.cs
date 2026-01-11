using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

namespace NoSideEffects
{
    public class Lightswitch : MonoBehaviour
    {
        public List<Light> lights;
        private bool isOn = true;

        [SerializeField] private Material materialToSwapFrom; // the material you want to replace
        [SerializeField] private Material materialToSwapTo;   // the new material
        [SerializeField] private bool changeChildrenMaterials = true;

        private List<Renderer> renderers = new List<Renderer>();

        private void Awake()
        {
            if (changeChildrenMaterials)
            {
                renderers.AddRange(GetComponentsInChildren<Renderer>(true)); // include inactive
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                if (rend != null) renderers.Add(rend);
            }
        }

        public void ToggleLights()
        {
            isOn = !isOn;
            foreach (var rootLight in lights)
            {
                if (rootLight == null)
                    continue;

                // Find all Light components in the root's children (include inactive)
                var childLights = rootLight.GetComponentsInChildren<Light>(true);
                for (int i = 0; i < childLights.Length; ++i)
                {
                    var child = childLights[i];
                    // If this child is a Point light, toggle it.
                    if (child != null && child.type == UnityEngine.LightType.Point)
                    {
                        child.enabled = isOn;
                    }
                }
            }
        }

        public void SetLights(bool on)
        {
            isOn = on;
            foreach (var light in lights)
                if (light != null) light.enabled = isOn;
        }

        public void TurnOffLights() => SetLights(false);
        public void TurnOnLights() => SetLights(true);

        public void ChangeMaterials()
        {
            if (materialToSwapFrom == null || materialToSwapTo == null)
            {
                Debug.LogWarning("Assign both materials to swap!");
                return;
            }

            int changedCount = 0;

            foreach (var rend in renderers)
            {
                if (rend == null) continue;

                Material[] mats = rend.sharedMaterials;

                for (int i = 0; i < mats.Length; i++)
                {
                    // Swap only the materials that match the original
                    if (mats[i] == materialToSwapFrom || mats[i].name.StartsWith(materialToSwapFrom.name))
                    {
                        mats[i] = materialToSwapTo;
                        changedCount++;
                    }
                }

                rend.sharedMaterials = mats;
            }

            Debug.Log($"Changed {changedCount} specific materials in hierarchy.");
        }
        public void ResetMaterials()
        {
            if (materialToSwapFrom == null || materialToSwapTo == null)
            {
                Debug.LogWarning("Assign both materials to swap!");
                return;
            }
            int changedCount = 0;
            foreach (var rend in renderers)
            {
                if (rend == null) continue;
                Material[] mats = rend.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    // Swap back only the materials that match the swapped one
                    if (mats[i] == materialToSwapTo || mats[i].name.StartsWith(materialToSwapTo.name))
                    {
                        mats[i] = materialToSwapFrom;
                        changedCount++;
                    }
                }
                rend.sharedMaterials = mats;
            }
            Debug.Log($"Reset {changedCount} specific materials in hierarchy.");
        }
    }
}
