// OLD VERSION WITHOUT CINEMACHINE
//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
//using Cursor = UnityEngine.Cursor;

//namespace NoSideEffects
//{
//    public class PlayerController : MonoBehaviour
//    {
//        [NonSerialized] public Rigidbody rigid_Body;
//        [SerializeField] Transform Player;
//        [SerializeField] Transform Head;
//        [SerializeField] Transform FPViewCamera;
//        InputAction moveAction, lookAction, interactButton;
//        [NonSerialized] public float playerSpeed = 3f;
//        [NonSerialized] public bool canMove = false;
//        private Vector2 moveValue;
//        private Vector2 lookValue;
//        // Start is called once before the first execution of Update after the MonoBehaviour is created
//        void Awake()
//        {
//            rigid_Body = GetComponent<Rigidbody>();
//            Player = GetComponent<Transform>();
//            // Head = GetComponent<Transform>();
//            moveAction = InputSystem.actions.FindAction("Move");
//            lookAction = InputSystem.actions.FindAction("Look");
//            interactButton = InputSystem.actions.FindAction("Interact");
//        }

//        private void Start()
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//            RiseFromBed();
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            moveValue = moveAction.ReadValue<Vector2>();
//            lookValue = lookAction.ReadValue<Vector2>();

//            Vector3 camForward = FPViewCamera.forward;
//            Vector3 camRight = FPViewCamera.right;

//            camForward.y = 0;
//            camRight.y = 0;

//            Vector3 forwardRelative = camForward * moveValue.y;
//            Vector3 rightRelative = camRight * moveValue.x;

//            if (rigid_Body.angularVelocity.magnitude > 0f)
//                rigid_Body.angularVelocity = Vector3.zero;

//            if (Head.eulerAngles.z > 0f)
//            {
//                Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 0);
//                Debug.Log("Corrected Head.eulerAngles.z");
//            }

//            Vector3 relativeMoveDirection = forwardRelative + rightRelative;
//            Vector3 projected = Vector3.ProjectOnPlane(relativeMoveDirection, Vector3.up);

//            //RaycastHit hit;
//            //int layerMask = LayerMask.GetMask("Default");

//            if (canMove)
//                Player.position += projected * playerSpeed * Time.deltaTime;

//            Player.rotation *= Quaternion.Euler(0f, lookValue.x, 0f);

//            if (lookValue.y > 3)
//                lookValue.y = 3;
//            else if (lookValue.y < -3)
//                lookValue.y = -3;

//            Quaternion testRotation = Head.rotation * Quaternion.Euler(-lookValue.y, 0f, 0f);

//            if ((testRotation.eulerAngles.x > 0 && testRotation.eulerAngles.x < 75) || (testRotation.eulerAngles.x < 360 && testRotation.eulerAngles.x > 280))
//                Head.rotation *= Quaternion.Euler(-lookValue.y, 0f, 0f);

//            // Debug.Log("lookValue.y: " + lookValue.y);
//        }
//        public void RiseFromBed()
//        {
//            // Head.localRotation = Quaternion.Euler(-75f, 0f, 0f);
//            canMove = true;
//            // DSM.instance.EndingDay(DSM.Days.Day2);

//            HUD.instance.SetUniqueItemText("Waking up on " + DSM.instance.GetCurrentDayString());
//            StartCoroutine(HUD.instance.TextTimerCoroutine(4f));
//        }
//    }
//}

//CINEMACHINE VERSION
using System;
using Unity.Cinemachine;
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

        // Cinemachine input controller(found at runtime)
        CinemachineInputAxisController inputAxisController;

        void Awake()
        {
            rigid_Body = GetComponent<Rigidbody>();
            Player = GetComponent<Transform>();
            // Head = GetComponent<Transform>();
            // cameraRotation = GetComponent<CinemachineCamera>();
            moveAction = InputSystem.actions.FindAction("Move");
            interactButton = InputSystem.actions.FindAction("Interact");

            if (inputAxisController == null)
                inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
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

            // ToggleCameraPanOn();
            // ToggleCameraTiltOn();

            HUD.instance.SetUniqueItemText("Waking up on " + DSM.instance.GetCurrentDayString());
            StartCoroutine(HUD.instance.TextTimerCoroutine(4f));
        }
        public void SetControllerEnabledByName(string axisName, bool enabled)
        {
            if (inputAxisController == null)
                inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
            if (inputAxisController == null)
            {
                Debug.LogWarning("No CinemachineInputAxisController found to modify controllers.");
                return;
            }

            inputAxisController.SynchronizeControllers();

            var controller = inputAxisController.GetController(axisName);
            if (controller != null)
            {
                controller.Enabled = enabled;
                return;
            }

            // Fallback: try case-insensitive contains search
            foreach (var c in inputAxisController.Controllers)
            {
                if (c == null || string.IsNullOrEmpty(c.Name)) continue;
                if (c.Name.IndexOf(axisName, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    c.Enabled = enabled;
                    return;
                }
            }

            Debug.LogWarning($"Controller with name '{axisName}' not found. Use LogControllerNames() to inspect available names.");
        }

        public void ToggleCameraPanOff()
        {
            SetControllerEnabledByName("Look X", false);
        }
        public void ToggleCameraPanOn()
        {
            SetControllerEnabledByName("Look X", true);
        }
        public void ToggleCameraTiltOff()
        {
            SetControllerEnabledByName("Look Y", false);
        }
        public void ToggleCameraTiltOn()
        {
            SetControllerEnabledByName("Look Y", true);
        }
        public void ToggleCameraRotationOff()
        {
            SetControllerEnabledByName("Look X", false);
            SetControllerEnabledByName("Look Y", false);
        }
        public void ToggleCameraRotationOn()
        {
            SetControllerEnabledByName("Look X", true);
            SetControllerEnabledByName("Look Y", true);
            canMove = true;
        }
    }
}
