using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f; // Movement speed
    private Vector2 movementInput; // Stores input from the New Input System
    private Animator animator;
    private bool isStrafing = false; // Tracks if the player is strafing
    private Camera mainCamera; // Reference to the main camera

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found in children!");
        }
    }

    private void Update()
    {
        Vector3 inputDirection = new Vector3(movementInput.x, 0f, movementInput.y);

        if (inputDirection.magnitude > 0.1f)
        {
            if (isStrafing)
            {
                // Move relative to facing direction, no rotation
                Vector3 moveDirection = (transform.forward * movementInput.y) + (transform.right * movementInput.x);
                moveDirection.Normalize();
                transform.position += moveDirection * speed * Time.deltaTime;
                // No rotation while strafing
            }
            else
            {
                // Rotate player to face input direction (world space)
                Vector3 moveDirection = inputDirection.normalized;
                transform.position += moveDirection * speed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Update animator with current movement state
        UpdateAnimationStates(inputDirection);
    }

    private void UpdateAnimationStates(Vector3 movement)
    {
        if (animator == null) return;

        float speedMagnitude = movement.magnitude;

        bool strafeLeft = false;
        bool strafeRight = false;
        bool walkBackwards = false;

        if (isStrafing)
        {
            if (movementInput.x < -0.1f)
                strafeLeft = true;
            else if (movementInput.x > 0.1f)
                strafeRight = true;
            else if (movementInput.y < -0.1f)
                walkBackwards = true;
        }

        animator.SetFloat("Speed", speedMagnitude);
        animator.SetBool("StrafeLeft", strafeLeft);
        animator.SetBool("StrafeRight", strafeRight);
        animator.SetBool("WalkBackwards", walkBackwards);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnRightClick(InputValue value)
    {
        isStrafing = value.isPressed;
    }
}