using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement configuration")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float movementSpeed = 10f;
    public float MovementSpeed { set { movementSpeed = value; } }

    Vector2 movement = new Vector2(0,0);
    public Vector2 Movement { get { return movement;} }

    [Tooltip("The amount that the players current velocity gets multiplied by at the start")]
    [SerializeField] float pushForceMultiplier = 5.0f;
    [Tooltip("The amount that the players current velocity gets divided by after push")]
    [SerializeField] float pushVelocityDivider = 4.0f;
    [Tooltip("The amount that the players current velocity gets divided by after jump")]
    [SerializeField] float jumpVelocityDivider = 4.0f;

    AssignedController controller;
    RotationController rotationController;
    Rigidbody2D rb2d;
    Player player;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        controller = GetComponent<AssignedController>();
        rotationController = GetComponent<RotationController>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {   
        if (!player.FreezeInput)
            MovePlayer();
    }

    private void MovePlayer()
    {
        movement = GetMovement();
        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;
        rb2d.AddForce(movement * movementSpeed);

        if (rb2d.velocity.magnitude > maxSpeed)
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
    }
    private Vector2 GetMovement()
    {
        Vector2 m_inputLeft = new Vector2(Input.GetAxis(controller.Horizontal), Input.GetAxis(controller.Vertical));
        
        if(m_inputLeft.magnitude < DeadZones.DEADZONE_LEFT)
            m_inputLeft = Vector2.zero;
        
        return m_inputLeft;
    }

    public void ResetMovement()
    {
        StopAllCoroutines();
    }

    //AnimationEvents:
    public void AddPushForce()
    {
        if (!player.FreezeInput)
            rb2d.AddRelativeForce(rotationController.Dir * pushForceMultiplier * 10, ForceMode2D.Impulse);
    }

    public void ReduceVelocity()
    {
        if (!player.FreezeInput)
            rb2d.velocity = rb2d.velocity / pushVelocityDivider;
    }
    
    public void ReduceVelocityAfterJump()
    {
        if (!player.FreezeInput)
            rb2d.velocity = rb2d.velocity / jumpVelocityDivider;
    }
}