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
        [SerializeField] Transform Head;
        [SerializeField] Transform FPViewCamera;
        InputAction moveAction, interactButton;
        [NonSerialized] public float playerSpeed = 3f;
        private Vector2 moveValue;
        private Vector2 lookValue;

        void Awake()
        {
            rigid_Body = GetComponent<Rigidbody>();
            Player = GetComponent<Transform>();
            Head = GetComponent<Transform>();
            moveAction = InputSystem.actions.FindAction("Move");
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
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
            Player.position += projected * playerSpeed * Time.deltaTime;

            //Debug.Log("");
        }
    }
}
