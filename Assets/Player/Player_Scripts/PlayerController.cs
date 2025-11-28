using System;
// using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

namespace NoSideEffects
{
    public class PlayerController : MonoBehaviour
    {
        [NonSerialized] public Rigidbody rigid_Body;
        [SerializeField] Transform Player;
        [SerializeField] Transform Head;
        [SerializeField] Transform FPViewCamera;
        // [SerializeField] CinemachineCamera cameraRotation;
        InputAction moveAction, interactButton;
        [NonSerialized] public float playerSpeed = 3f;
        [NonSerialized] public bool canMove, cameraLocked = false;
        private Vector2 moveValue;
        private Vector2 lookValue;

        // Cinemachine input controller (found at runtime)
        // CinemachineInputAxisController inputAxisController;

        void Awake()
        {
            rigid_Body = GetComponent<Rigidbody>();
            Player = GetComponent<Transform>();
            // Head = GetComponent<Transform>();
            // cameraRotation = GetComponent<CinemachineCamera>();
            moveAction = InputSystem.actions.FindAction("Move");
            interactButton = InputSystem.actions.FindAction("Interact");

            //if (inputAxisController == null)
            //    inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        }
        private void Start()
        {
            // cameraLocked = true;
            RiseFromBed();
        }
        void Update()
        {
            moveValue = moveAction.ReadValue<Vector2>();

            Vector3 camForward = FPViewCamera.forward;
            Vector3 camRight = FPViewCamera.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 forwardRelative = camForward * moveValue.y;
            Vector3 rightRelative = camRight * moveValue.x;

            if (rigid_Body.angularVelocity.magnitude > 0f)
                rigid_Body.angularVelocity = Vector3.zero;

            if (Head.eulerAngles.z > 0f)
                Head.eulerAngles = Vector3.zero;

            Vector3 relativeMoveDirection = forwardRelative + rightRelative;

            //RaycastHit hit;
            //int layerMask = LayerMask.GetMask("Default");

            // Vector3 moveDirection = new Vector3(moveValue.x, 0f, moveValue.y);
            Vector3 projected = Vector3.ProjectOnPlane(relativeMoveDirection, Vector3.up);
            if (canMove)
                Player.position += projected * playerSpeed * Time.deltaTime;

            //Debug.Log("");
        }
        public void RiseFromBed()
        {
            // Head.localRotation = Quaternion.Euler(-75f, 0f, 0f);
            canMove = true;
            // DSM.instance.EndingDay(DSM.Days.Day2);

            //ToggleCameraPanOn();
            //ToggleCameraTiltOn();

            HUD.instance.SetUniqueItemText("Waking up on " + DSM.instance.GetCurrentDayString());
            StartCoroutine(HUD.instance.TextTimerCoroutine(4f));
        }
        //public void SetControllerEnabledByName(string axisName, bool enabled)
        //{
        //    if (inputAxisController == null)
        //        inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        //    if (inputAxisController == null)
        //    {
        //        Debug.LogWarning("No CinemachineInputAxisController found to modify controllers.");
        //        return;
        //    }

        //    inputAxisController.SynchronizeControllers();

        //    var controller = inputAxisController.GetController(axisName);
        //    if (controller != null)
        //    {
        //        controller.Enabled = enabled;
        //        return;
        //    }

        //    // Fallback: try case-insensitive contains search
        //    foreach (var c in inputAxisController.Controllers)
        //    {
        //        if (c == null || string.IsNullOrEmpty(c.Name)) continue;
        //        if (c.Name.IndexOf(axisName, StringComparison.InvariantCultureIgnoreCase) >= 0)
        //        {
        //            c.Enabled = enabled;
        //            return;
        //        }
        //    }

        //    Debug.LogWarning($"Controller with name '{axisName}' not found. Use LogControllerNames() to inspect available names.");
        //}
        
        //public void ToggleCameraPanOff()
        //{
        //    SetControllerEnabledByName("Look X", false);
        //}
        //public void ToggleCameraPanOn()
        //{
        //    SetControllerEnabledByName("Look X", true);
        //}
        //public void ToggleCameraTiltOff()
        //{
        //    SetControllerEnabledByName("Look Y", false);
        //}
        //public void ToggleCameraTiltOn()
        //{
        //    SetControllerEnabledByName("Look Y", true);
        //}
        //public void ToggleCameraRotationOff()
        //{
        //    SetControllerEnabledByName("Look X", false);
        //    SetControllerEnabledByName("Look Y", false);
        //}
        //public void ToggleCameraRotationOn()
        //{
        //    SetControllerEnabledByName("Look X", true);
        //    SetControllerEnabledByName("Look Y", true);
        //    canMove = true;
        //}
    }
}
