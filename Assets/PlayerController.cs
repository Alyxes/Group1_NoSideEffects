using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
            FPViewCamera.rotation *= Quaternion.Euler(-lookValue.y, 0f, 0f);

            Debug.Log("FPViewCamera.rotation: " + FPViewCamera.eulerAngles.x);
        }
    }
}
