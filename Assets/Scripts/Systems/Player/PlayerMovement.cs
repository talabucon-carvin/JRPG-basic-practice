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
        // Cache the main camera reference
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
        // Convert the Vector2 input into a Vector3 for movement
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y);

        if (movement.magnitude > 0.1f) // Only process movement if input is significant
        {
            transform.position += movement * speed * Time.deltaTime;

            if (!isStrafing)
            {
                // Rotate only if not strafing
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Update animator
        UpdateAnimationStates(movement);

    }

    private void UpdateAnimationStates(Vector3 movement)
    {
        if (animator == null) return;

        float speedMagnitude = movement.magnitude;

        // Reset all directional booleans
        bool strafeLeft = false;
        bool strafeRight = false;
        bool walkBackwards = false;

        // Determine specific strafe direction
        if (isStrafing)
        {
            if (movementInput.x < -0.1f) // A key
                strafeLeft = true;
            else if (movementInput.x > 0.1f) // D key
                strafeRight = true;
            else if (movementInput.y < -0.1f) // S key
                walkBackwards = true;
        }

        // Set animator params
        animator.SetFloat("Speed", speedMagnitude);
        animator.SetBool("StrafeLeft", strafeLeft);
        animator.SetBool("StrafeRight", strafeRight);
        animator.SetBool("WalkBackwards", walkBackwards);
    }


    // This method is called by the Input System when the Move action is triggered
    public void OnMove(InputValue value)
    {
        // Debug.Log("On Move");
        movementInput = value.Get<Vector2>();

    }

    // This method is called by the Input System when the Right Mouse Button is pressed or released
    public void OnRightClick(InputValue value)
    {

       isStrafing = value.isPressed;

    }
    // public void OnRightClick(InputAction.CallbackContext context)
    // {
    //         Debug.Log("is this even being called");

    //     if (context.started)
    //     {
    //         isStrafing = true; // Start strafing
    //     }
    //     else if (context.canceled)
    //     {
    //         isStrafing = false; // Stop strafing
    //     }
    //         Debug.Log("Right click held: " + isStrafing);
    // }
}

