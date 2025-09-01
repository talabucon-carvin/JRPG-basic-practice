using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    private Vector2 movementInput; // Stores input from the New Input System
    private Animator animator;

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

        // Move the character by modifying its position
        transform.position += movement * speed * Time.deltaTime;
        animator.SetFloat("Speed", movementInput.magnitude);

    }

    // This method is called by the Input System when the Move action is triggered
    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}

