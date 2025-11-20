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

            //RaycastHit hit;
            //int layerMask = LayerMask.GetMask("Default");

            Vector3 moveDirection = new Vector3(moveValue.x, 0, moveValue.y);
            transform.position += moveDirection * playerSpeed * Time.deltaTime;
        }
    }
}
