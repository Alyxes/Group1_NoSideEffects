using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

namespace NoSideEffects
{
    public class PlayerController : MonoBehaviour
    {
        [NonSerialized] public Rigidbody rigid_Body;
        [SerializeField] Transform Player;
        [SerializeField] Transform FPViewCamera;
        InputAction moveAction, lookAction, interactButton;
        [NonSerialized] public float playerSpeed = 3f;
        private Vector2 moveValue;
        private Vector2 lookValue;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            rigid_Body = GetComponent<Rigidbody>();
            Player = GetComponent<Transform>();
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            moveValue = moveAction.ReadValue<Vector2>();
            lookValue = lookAction.ReadValue<Vector2>();

            Vector3 camForward = FPViewCamera.forward;
            Vector3 camRight = FPViewCamera.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 forwardRelative = camForward * moveValue.y;
            Vector3 rightRelative = camRight * moveValue.x;

            if (rigid_Body.angularVelocity.magnitude > 0f)
                rigid_Body.angularVelocity = Vector3.zero;

            Vector3 relativeMoveDirection = forwardRelative + rightRelative;

            //RaycastHit hit;
            //int layerMask = LayerMask.GetMask("Default");

            Player.position += relativeMoveDirection * playerSpeed * Time.deltaTime;
            Player.rotation *= Quaternion.Euler(0f, lookValue.x, 0f);

            if (lookValue.y > 3)
                lookValue.y = 3;
            else if (lookValue.y < -3)
                lookValue.y = -3;

            Quaternion testRotation = FPViewCamera.rotation * Quaternion.Euler(-lookValue.y, 0f, 0f);

            if ((testRotation.eulerAngles.x > 0 && testRotation.eulerAngles.x < 75) || (testRotation.eulerAngles.x < 360 && testRotation.eulerAngles.x > 280))
                FPViewCamera.rotation *= Quaternion.Euler(-lookValue.y, 0f, 0f);

            Debug.Log("lookValue.y: " + lookValue.y);
        }
    }
}
