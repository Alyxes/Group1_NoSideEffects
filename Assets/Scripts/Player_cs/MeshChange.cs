using System.Collections.Generic;
using UnityEngine;
// using static Unity.VisualScripting.Metadata;

namespace NoSideEffects
{
    public class MeshChange : MonoBehaviour
    {
        // Jag tog bort detta, då det gör denna kod oanvändbar för andra objekt än just dag 1-pusslet.
        // Istället är den döda plantan avaktiverad i editorn från början.
        //private void Start()
        //{
        //    DeactivateChild("plant_dead");
        //}

        [Header("Child GameObjects")]
        public List<GameObject> children = new List<GameObject>();
        public void SetChildrenActive(bool active)
        {
            foreach (GameObject child in children)
            {
                child.SetActive(active);
            }
        }
        public void ActivateChild(string childName)
        {
            Transform child = transform.Find(childName); // searches the children of this parent

            if (child != null)
            {
                child.gameObject.SetActive(true);  // turn on
            }
        }
        public void DeactivateChild(string childName)
        {
            Transform child = transform.Find(childName);

            if (child != null)
            {
                child.gameObject.SetActive(false);  // turn off
            }
        }
    }
}
