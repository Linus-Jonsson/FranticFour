using UnityEngine;
using System.Threading.Tasks;

public class MovementController : MonoBehaviour
{
    [Header("Movement configuration")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float movementSpeed = 10f;
    public float MovementSpeed { set { movementSpeed = value; } }

    Vector2 movement = new Vector2(0,0);
    public Vector2 Movement { get { return movement;} }

    [Tooltip("The maximum amount that speed can increase base on distance to prey")]
    [SerializeField] float maxSpeedIncrease = 50;
    [Tooltip("The distance that the speed boost starts to take effect")]
    [SerializeField] float minimumBoostDistance = 5f;

    [SerializeField] float distance = 0;

    [SerializeField] float speedBoost = 0;

    [Tooltip("The amount that the players current velocity gets multiplied by at the start")]
    [SerializeField] float pushForceMultiplier = 5.0f;

    AssignedController controller;
    RotationController rotationController;
    Rigidbody2D rb2d;
    Player player;
    InGameLoopController gameLoopController;

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
        gameLoopController = FindObjectOfType<InGameLoopController>();
    }

    void FixedUpdate()
    {
        if (player.FreezeInput)
            return;
        CalculateSpeedBoost();
        MovePlayer();
    }

    private void CalculateSpeedBoost()
    {
        Player prey = gameLoopController.CurrentPrey;
        distance = Vector2.Distance(transform.position, prey.transform.position);
        if (distance > minimumBoostDistance)
        {
            speedBoost = distance * 2;
            if (speedBoost > maxSpeedIncrease)
                speedBoost = maxSpeedIncrease;
        }
        else
        {
            speedBoost = 0;
        }
    }

    private void MovePlayer()
    {
        movement = GetMovement();
        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;

        rb2d.AddForce(movement * (movementSpeed + speedBoost));

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
    
    public void ReduceVelocityAfterJump()
    {
        player.FreezeInput = true;
        rb2d.velocity = Vector2.zero;
    }

    public void AddPushForce()
    {
        rb2d.AddForce(rotationController.Dir * pushForceMultiplier, ForceMode2D.Impulse);
    }
}