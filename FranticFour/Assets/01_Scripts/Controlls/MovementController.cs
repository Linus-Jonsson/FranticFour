using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement configuration")]
    [SerializeField] float movementSpeed = 10f;
    public float MovementSpeed { set { movementSpeed = value; } }

    [Tooltip("The amount that the players current velocity gets multiplied by at the start")]
    [SerializeField] float pushForceMultiplier = 5.0f;
    [Tooltip("The amount that the players current velocity gets divided by after push")]
    [SerializeField] float pushVelocityDivider = 4.0f;

    AssignedController controller;
    Rigidbody2D rb2d;
    PlayerAnimationsController playerAnimationsController;
    Player player;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        controller = GetComponent<AssignedController>();
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimationsController = GetComponent<PlayerAnimationsController>();
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (player.FreezeInput)
            return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 movement = GetMovement();
        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;
        playerAnimationsController.SetMovement(movement);
        rb2d.AddForce(movement * movementSpeed);
    }
    private Vector2 GetMovement()
    {
        float xMovement = Input.GetAxis(controller.Horizontal);
        float yMovement = Input.GetAxis(controller.Vertical);
        return new Vector2(xMovement, yMovement);
    }

    public void ResetMovement()
    {
        StopAllCoroutines();
        player.FreezeInput = false;
    }

    //AnimationEvents:
    public void AddPushForce()
    {
        if (!player.FreezeInput)
            rb2d.AddRelativeForce(Vector2.up * pushForceMultiplier, ForceMode2D.Impulse);
    }

    public void ReduceVelocity()
    {
        if (!player.FreezeInput)
            rb2d.velocity = rb2d.velocity / pushVelocityDivider;
    }
}