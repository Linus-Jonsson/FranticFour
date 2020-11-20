using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Layer configuration")]
    [Tooltip("Set this to have the layerNumber of the layer that player is")]
    [SerializeField] int playerLayer = 8;
    [Tooltip("Set this to have the layerNumber of the layer that Jump is")]
    [SerializeField] int jumpLayer = 9;

    [Header("Movement configuration")]
    [SerializeField] float movementSpeed = 10f;

    [SerializeField] float pushDuration = 0.3f;

    [Header("Jump configuration")]
    [Tooltip("The duration of the jump in seconds")]
    [SerializeField] float jumpDuration = 2f;
/*    [Tooltip("The cooldown on jumping in seconds (starts to count after jump is finished)")]
    [SerializeField] float jumpCooldown = 2f;*/
    [Tooltip("The drag on the rigidBody while jumping (This should be low due to no force applied during the jump")]
    [SerializeField] float jumpingDrag = 0.3f;
    
    [SerializeField] public AssignedController controller; //SerializeField + Public?

    Vector2 dir = new Vector2(0, 0);
    public Vector2 Dir { get { return dir; } }

    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    bool freezeInput = false;
    public bool FreezeInput { get { return freezeInput; } }
    
    bool canJump = true;

    float originalDrag;
    
    void Start()
    {
        controller = GetComponent<AssignedController>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalDrag = rb2d.drag;
    }

    private void Update()
    {
        if (canJump && Input.GetButton(controller.Jump) && !freezeInput)
            StartCoroutine(HandleJump());
    }

    void FixedUpdate()
    {
        if (freezeInput)
            return;

        MovePlayer();
        HandleRotation();
    }

    private void MovePlayer()
    {
        Vector2 movement = new Vector2(Input.GetAxis(controller.Horizontal), Input.GetAxis(controller.Vertical)).normalized;
        rb2d.AddForce(movement * movementSpeed);
    }

    IEnumerator HandleJump()
    {
        StartJumping();
        yield return new WaitForSeconds(jumpDuration);
        EndJumping();
        //yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void StartJumping()
    {
        rb2d.freezeRotation = true;
        canJump = false;
        gameObject.layer = jumpLayer;
        rb2d.drag = jumpingDrag;
        freezeInput = true;
        transform.localScale = new Vector3(transform.localScale.x + 2, transform.localScale.y + 2, 5); // remove this once we have animation
    }
    
    private void EndJumping()
    {
        rb2d.freezeRotation = false;
        gameObject.layer = playerLayer;
        rb2d.drag = originalDrag;
        freezeInput = false;
        transform.localScale = new Vector3(transform.localScale.x - 2, transform.localScale.y - 2, 5); // remove this once we have animation
    }

    private void HandleRotation()
    {
        // add a small check if controller is assigned or not and use an if statement to controll what rotation to use.
        HandleControllerRotation();
        //HandleMouseRotation(); Musen skriver över kontroller inputs
    }

    private void HandleControllerRotation()
    {
        float inputX = Input.GetAxis(controller.RightHorizontal);
        float inputY = Input.GetAxis(controller.RightVertical);

        if (inputX != 0)
            dir.x = inputX;
        if (inputY != 0)
            dir.y = inputY;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
    
    private void HandleMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
    
    public void GetPushed(Vector2 pushForce)
    {
        StartCoroutine(HandlePush(pushForce));
    }

    IEnumerator HandlePush(Vector2 pushForce)
    {
        freezeInput = true;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(pushDuration);
        freezeInput = false;
    }

    public void GetStunned(float duration)
    {
        StartCoroutine(HandleStun(duration));
    }

    IEnumerator HandleStun(float duration)
    {
        freezeInput = true;
        StunBlink();
        yield return new WaitForSeconds(duration);
        freezeInput = false;
        spriteRenderer.color = originalColor;
    }

    private async void StunBlink()
    {
        while (freezeInput)
        {
            await Task.Delay(100);
            spriteRenderer.color = spriteRenderer.color == originalColor ? Color.white : originalColor;
        }
    }
}
