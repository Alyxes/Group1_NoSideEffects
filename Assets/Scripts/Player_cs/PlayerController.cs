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
//            // DSM.instance.StartingNewDay(DSM.Days.Day2);

//            HUD.instance.SetUniqueItemText("Waking up on " + DSM.instance.GetCurrentDayString());
//            StartCoroutine(HUD.instance.PickUpTimeOutCoroutine(4f));
//        }
//    }
//}

//CINEMACHINE VERSION
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Windows;
using Cursor = UnityEngine.Cursor;

namespace NoSideEffects
{
    public class PlayerController : MonoBehaviour
    {
        [NonSerialized] public Rigidbody rigid_Body;
        [SerializeField] Transform Player;
        [SerializeField] GameObject playerBody;
        [SerializeField] Transform Head;
        [SerializeField] Transform FPViewCamera;
        public GameObject flashlight;
        InputAction moveAction, crouchButton;
        [NonSerialized] public float playerDaySpeed = 1.5f;
        [NonSerialized] public float playerSpeed;
        [NonSerialized] public bool canMove, wakingUp, isCrouching, isMovingFurniture = false;
        [NonSerialized] public Vector2 moveValue;
        private Vector3 projected;
        private float wantedHeadHeight;
        private float currentHeadXrotation;
        private float wantedHeadXrotation;
        private float currentHeadYrotation;
        private float wantedHeadYrotation;
        private float currentXposition;
        private float wantedXposition;
        private float currentZposition;
        private float wantedZposition;

        // Cinemachine input controller(found at runtime)
        CinemachineInputAxisController inputAxisController;

        void Awake()
        {
            flashlight.SetActive(false);
            rigid_Body = GetComponent<Rigidbody>();
            Player = GetComponent<Transform>();
            // Head = GetComponent<Transform>();
            wantedHeadHeight = Head.position.y;
            // cameraRotation = GetComponent<CinemachineCamera>();
            moveAction = InputSystem.actions.FindAction("Move");
            // interactButton = InputSystem.actions.FindAction("Interact");
            crouchButton = InputSystem.actions.FindAction("Crouch");

            if (inputAxisController == null)
                inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerSpeed = playerDaySpeed;

            ToggleCameraRotationOff();
            HUD.instance.SetBlackScreenAlpha(1);
            SetPlayerHeightCrouching(true, false);
            Head.localRotation = Quaternion.Euler(285f, 0f, 0f);

            currentHeadXrotation = 285f;
            wantedHeadXrotation = 360f;
            currentHeadYrotation = 270f;
            wantedHeadYrotation = 270f;

            currentXposition = transform.localPosition.x;
            currentZposition = transform.localPosition.z;
            wantedXposition = currentXposition;
            wantedZposition = currentZposition;

            Awakening();
        }
        void Update()
        {
            if (wakingUp)
            {
                RiseFromBedAnimation();
                return;
            }

            if (canMove && crouchButton.WasPressedThisFrame())
            {
                ToggleCrouch();
            }

            moveValue = moveAction.ReadValue<Vector2>();

            // Computing planar camera axes. This makes sure that movement force is correctly applied even when looking up or down.
            Vector3 camForward = Vector3.ProjectOnPlane(FPViewCamera.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(FPViewCamera.right, Vector3.up).normalized;

            Vector3 forwardRelative = camForward * moveValue.y;
            Vector3 rightRelative = camRight * moveValue.x;

            if (rigid_Body.angularVelocity.magnitude > 0f)
                rigid_Body.angularVelocity = Vector3.zero;

            if (Head.eulerAngles.z > 0f)
                Head.eulerAngles = Vector3.zero;

            Vector3 relativeMoveDirection = forwardRelative + rightRelative;

            projected = Vector3.ProjectOnPlane(relativeMoveDirection, Vector3.up);

            //Debug.Log("");
        }
        void FixedUpdate()
        {
            if (canMove)
            {
                rigid_Body.AddForce(projected * 800f * Time.fixedDeltaTime * playerSpeed, ForceMode.Impulse);
            }

            if (wantedHeadHeight != Head.position.y)
            {
                Head.position = Vector3.MoveTowards(Head.position, new Vector3(Head.position.x, wantedHeadHeight, Head.position.z), Time.fixedDeltaTime * 2f);
            }

            DampingPlanarMovement(0.9f);
        }
        public void ToggleCrouch()
        {
            if (canMove)
            {
                if (!isCrouching)
                {
                    isCrouching = true;
                    SetPlayerHeightCrouching(false, true);
                    playerSpeed = 0.8f;
                }
                else
                {
                    isCrouching = false;
                    SetPlayerHeightNormal(false, true);
                    playerSpeed = playerDaySpeed;
                }
            }
        }
        public void SetPlayerHeightNormal(bool setHeadPosition, bool initiateHeadLerp)
        {
            playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
            playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);
            if (setHeadPosition || initiateHeadLerp)
                SetPlayerHeadNormal(setHeadPosition, initiateHeadLerp);
        }
        public void SetPlayerHeadNormal(bool setHeadPosition, bool initiateHeadLerp)
        {
            if (setHeadPosition)
                Head.position = new Vector3(Head.position.x, 1.785f, Head.position.z);
            if (initiateHeadLerp)
                wantedHeadHeight = 1.785f;
        }
        public void SetPlayerHeightCrouching(bool setHeadPosition, bool initiateHeadLerp)
        {
            playerBody.GetComponent<CapsuleCollider>().height = 1f;
            playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.378f, 0f);
            if (setHeadPosition || initiateHeadLerp)
                SetPlayerHeadCrouching(setHeadPosition, initiateHeadLerp);
        }
        public void SetPlayerHeadCrouching(bool setHeadPosition, bool initiateHeadLerp)
        {
            if (setHeadPosition)
                Head.position = new Vector3(Head.position.x, 0.885f, Head.position.z);
            if (initiateHeadLerp)
                wantedHeadHeight = 0.885f;
        }
        public void SetPlayerHeightCrawling(bool setHeadPosition, bool initiateHeadLerp)
        {
            playerBody.GetComponent<CapsuleCollider>().height = 0.7f;
            playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.535f, 0f);
            if (setHeadPosition || initiateHeadLerp)
                SetPlayerHeadCrawling(setHeadPosition, initiateHeadLerp);
        }
        public void SetPlayerHeadCrawling(bool setHeadPosition, bool initiateHeadLerp)
        {
            if (setHeadPosition)
                Head.position = new Vector3(Head.position.x, 0.485f, Head.position.z);
            if (initiateHeadLerp)
                wantedHeadHeight = 0.485f;
        }
        public void SetHeadHeightSpecific(float _height, bool setHeadPosition, bool initiateHeadLerp)
        {
            if (setHeadPosition)
                Head.position = new Vector3(Head.position.x, _height, Head.position.z);
            if (initiateHeadLerp)
                wantedHeadHeight = _height;
        }
        public void RiseFromBedAnimation()
        {
            if (currentHeadXrotation < wantedHeadXrotation)
            {
                currentHeadXrotation += Time.deltaTime * 42f;
                Head.localRotation = Quaternion.Euler(currentHeadXrotation, 0f, 0f);
                if (currentHeadXrotation > 320f && wantedHeadYrotation == 270f)
                {
                    wantedHeadYrotation = 360f;
                }
            }
            else
            {
                currentHeadXrotation = wantedHeadXrotation;
                Head.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (currentHeadYrotation < wantedHeadYrotation)
            {
                currentHeadYrotation += Time.deltaTime * 64f;
                transform.localRotation = Quaternion.Euler(0f, currentHeadYrotation, 0f);

                if (currentHeadYrotation > 285f && wantedZposition == currentZposition)
                    wantedZposition = transform.localPosition.z + 1.5f;
            }
            else
            {
                currentHeadYrotation = wantedHeadYrotation;
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (currentXposition > wantedXposition)
            {
                currentXposition -= Time.deltaTime;
                transform.localPosition = new Vector3(currentXposition, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                currentXposition = wantedXposition;
                transform.localPosition = new Vector3(currentXposition, transform.localPosition.y, transform.localPosition.z);
            }

            if (currentZposition < wantedZposition)
            {
                currentZposition += Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentZposition);
            }
            else
            {
                currentZposition = wantedZposition;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentZposition);
            }

            if (currentHeadXrotation == wantedHeadXrotation && currentHeadYrotation == wantedHeadYrotation && currentZposition == wantedZposition)
            {
                SetPlayerHeightNormal(false, false);
                StartCoroutine(WaitAndRunFuncton(() => { ExitBed(); return null; }, 1f));
                wakingUp = false;
            }
        }
        public void DampingPlanarMovement(float amount)
        {
            rigid_Body.linearVelocity = new Vector3(rigid_Body.linearVelocity.x * amount, rigid_Body.linearVelocity.y, rigid_Body.linearVelocity.z * amount);
        }
        public IEnumerator WaitAndRunFuncton(Func<object> function, float time)
        {
            yield return new WaitForSeconds(time);
            function();
        }
        public void Awakening()
        {
            wakingUp = true;

            HUD.instance.SetUniqueItemText("Waking up on " + DSM.instance.GetCurrentDayString());
            StartCoroutine(HUD.instance.PickUpTimeOutCoroutine(6f));

            HUD.instance.blackScreenFadeOut = true;
            HUD.instance.blackScreenFadeSpeed = 0.4f;

            SetPlayerHeadNormal(false, true);
            wantedXposition = transform.localPosition.x - 1f;

            AudioManager.PlaySound(SoundType.GETTINGUPFROMBED, AudioManager.instance.audSrc_PlayerMovement);
        }
        public void ExitBed()
        {
            ToggleCameraRotationOn();
            canMove = true;

            // This text will be different or be none at all depending on the day.
            if (DSM.instance.wakeUpMonologue != "")
                StartCoroutine(HUD.instance.SetTimerUntilSubTitle(DSM.instance.monolougeTimer, DSM.instance.wakeUpMonologue));
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
        }
        public IEnumerator SwitchCameraRotationOnTimer(float time)
        {
            yield return new WaitForSeconds(time);
            ToggleCameraRotationOn();
        }
        public IEnumerator SwitchCameraRotationOffTimer(float time)
        {
            yield return new WaitForSeconds(time);
            ToggleCameraRotationOff();
        }
        public IEnumerator SwitchOnPlayerMovementTimer(float time)
        {
            yield return new WaitForSeconds(time);
            canMove = true;
        }
        public IEnumerator SwitchOffPlayerMovementTimer(float time)
        {
            yield return new WaitForSeconds(time);
            canMove = false;
        }
    }
}
