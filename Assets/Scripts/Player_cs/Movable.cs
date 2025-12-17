using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Movable : MonoBehaviour
    {
        public string furnitureName;
        public bool IsMovable = true;
        public bool IsMoving, isBeingPulled = false;
        public PlayerController player;
        public Transform playerPosition;
        public bool XAxis = false;
        public bool ZAxis = false;
        public Material mat,mat2;

        private bool inZone, isPlayingSound = false;
        private Vector3 OldPos;
        private Rigidbody furniture_rb;
        private InputAction interactButton;
        private InputAction ResetButton;
        private Vector3 OrgPos;
        private Vector3 playerOrgPos= new Vector3(5.87300014f, 0.93900001f, 9.96700001f);

        
        void Awake()
        {
            if (player == null)
                player = GetComponentInChildren<PlayerController>();
            
            playerPosition = player.transform;
            
            furniture_rb = GetComponent<Rigidbody>();
            OldPos = furniture_rb.position;
            interactButton = InputSystem.actions.FindAction("Interact");
            ResetButton = InputSystem.actions.FindAction("Reset");
            if(GetComponent<MeshRenderer>()!= null && (mat || mat2) ) {//check if null on stuff
                if (XAxis )
                {
                    Material[] mats = GetComponent<MeshRenderer>().materials;
                    mats[0] = mat;
                    GetComponent<MeshRenderer>().materials = mats;
                }
                else if (ZAxis )
                {
                    Material[] mats = GetComponent<MeshRenderer>().materials;
                    mats[0] = mat2;
                    GetComponent<MeshRenderer>().materials = mats;
                }
            }
                //furniture_rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                furniture_rb.constraints = 
                (XAxis? RigidbodyConstraints.FreezePositionZ :  RigidbodyConstraints.None)  | 
                (ZAxis? RigidbodyConstraints.FreezePositionX :  RigidbodyConstraints.None)  |
                 RigidbodyConstraints.FreezeRotation;

            OrgPos=transform.position;

        }

        void Reset()
        {
            transform.position = OrgPos;
            playerPosition.position = playerOrgPos;
        }

        // Update is called once per frame
        void Update()
        {
            if (ResetButton.IsPressed())
                {
                    Reset();
                }
            if (inZone)
            {
                
                if (interactButton.IsPressed())
                {
                    if (IsMovable)
                    {
                        EnablePulling();
                        HUD.instance.SetUniqueItemText("Pulling " + furnitureName);
                    }
                }
                else
                {
                    DisablePulling();
                    HUD.instance.SetUniqueItemText("Move towards to push - Interact to pull " + furnitureName);
                }

                if (isBeingPulled)
                {
                    Vector3 newPos = Vector3.MoveTowards(furniture_rb.position, playerPosition.position, player.moveValue.sqrMagnitude / 1.7f * player.playerSpeed * Time.deltaTime);
                    furniture_rb.MovePosition(newPos);
                }
            }
            
            Vector3 currentPos = furniture_rb.position;

            if (currentPos != OldPos)
            {
                if (!isPlayingSound)
                {
                    isPlayingSound = true;
                    Debug.Log("Scraping sound begin");
                    AudioManager.StartLoopingSound(SoundType.FURNITUREMOVE);
                }
                Debug.Log("Scraping sound continues");
                OldPos = currentPos;
            }
            else
            {
                EndFurnitureSounds();
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;
                    HUD.instance.SetUniqueItemText("Move towards to push - Interact to pull " + furnitureName);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (inZone)
                {
                    inZone = false;
                    Debug.Log("left the pushing zone");
                    DisablePulling();
                    EndFurnitureSounds();
                    // EnableKinematic();
                    HUD.instance.ClearPickUpText();
                }
            }
        }
        //public void DisableKinematic()
        //{
        //    furniture_rb.isKinematic = false;
        //}
        //public void EnableKinematic()
        //{
        //    furniture_rb.isKinematic = true;
        //}
        public void EnablePulling()
        {
            if (!isBeingPulled)
            {
                isBeingPulled = true;
                player.playerSpeed = player.playerSpeed/2.3f;
            }
        }
        public void DisablePulling()
        {
            if (isBeingPulled)
            {
                isBeingPulled = false;
                player.playerSpeed = player.playerDaySpeed;
            }
        }
        private void EndFurnitureSounds()
        {
            if (isPlayingSound)
            {
                isPlayingSound = false;
                Debug.Log("Scraping sound stops.");
                AudioManager.StopSound();
            }
        }
    }
}