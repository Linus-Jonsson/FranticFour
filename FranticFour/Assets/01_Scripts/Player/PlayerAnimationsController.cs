using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    Animator animator;
    RotationController rotationController;
    MovementController movementController;
    Rigidbody2D rb2d;

    void Start()
    {
        animator = GetComponent<Animator>();
        rotationController = GetComponent<RotationController>();
        movementController = GetComponent<MovementController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        SetVelocity();
        SetDirection();
        SetMovement();
    }

    private void SetVelocity()
    {
        animator.SetFloat("velocityX", rb2d.velocity.x);
        animator.SetFloat("velocityY", rb2d.velocity.y);
        animator.SetFloat("speed", rb2d.velocity.magnitude);
    }

    private void SetDirection()
    {
        Vector2 direction = rotationController.Dir.normalized;
        animator.SetFloat("directionX", direction.x);
        animator.SetFloat("directionY", direction.y);
    }
    
    private void SetMovement()
    {
        Vector2 movement = movementController.Movement;
        animator.SetFloat("movementX", movement.x);
        animator.SetFloat("movementY", movement.y);
    }
    
    public void ResetAnimationTrigger(string triggerName)
    {
        animator.ResetTrigger(triggerName);
    }
}
