using System;
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
    [Header("Push configuration")]
    [Tooltip("The time in seconds that the player getting pushed wont be able to move")]
    [SerializeField] float pushDuration = 0.3f;
    [Header("Jump configuration")] 
    [Tooltip("The duration of the jump in seconds")]
    [SerializeField] float jumpDuration = 2f;
    [Tooltip("The drag on the rigidBody while jumping (This should be low due to no force applied during the jump")]
    [SerializeField] float jumpingDrag = 0.3f;

    [Header("Other")]
    [SerializeField] public AssignedController controller; //SerializeField + Public?
    [SerializeField] Color originalColor;
    [SerializeField] GameObject body = null;

    Vector2 dir = new Vector2(0, 0);
    public Vector2 Dir { get { return dir; } }

    Rigidbody2D rb2d;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool freezeInput = false;

    public bool FreezeInput { get { return freezeInput; } set { freezeInput = value; } }
    public float MovementSpeed { set { movementSpeed = value; } }

    bool canJump = true;

    float originalDrag;
    
    void Start()
    {
        controller = GetComponent<AssignedController>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
        originalDrag = rb2d.drag;
    }

    private void Update()
    {
        if (canJump && Input.GetButton(controller.Jump) && !freezeInput)
            animator.SetTrigger("Jump");
    }

    void FixedUpdate()
    {
        if (freezeInput)
            return;

        MovePlayer();
        HandleRotation();
    }

    private void LateUpdate()
    {
        body.transform.rotation = Quaternion.Euler (0.0f, 0.0f, transform.rotation.z * -1.0f);
    }

    private void MovePlayer()
    {
        float xMovement = Input.GetAxis(controller.Horizontal);
        float yMovement = Input.GetAxis(controller.Vertical);
        Vector2 movement = new Vector2(xMovement, yMovement).normalized;
        SetRunAnimation(movement);
        rb2d.AddForce(movement * movementSpeed);
    }

    private void SetRunAnimation(Vector2 movement)
    {
        animator.SetFloat("movementX", movement.x);
        animator.SetFloat("movementY", movement.y);
        animator.SetFloat("speed", new Vector2(movement.x, movement.y).magnitude);
    }

    public void StartJumping()
    {
        canJump = false;
        rb2d.freezeRotation = true;
        canJump = false;
        gameObject.layer = jumpLayer;
        rb2d.drag = jumpingDrag;
        freezeInput = true;
    }
    public void EndJumping()
    {
        canJump = true;
        rb2d.freezeRotation = false;
        gameObject.layer = playerLayer;
        rb2d.drag = originalDrag;
        freezeInput = false;
    }

    private void HandleRotation()
    {
        if (controller.UsesMouse)
            HandleMouseRotation(); //Musen skriver över kontroller inputs
        else
            HandleControllerRotation();
    }

    private void HandleControllerRotation()
    {
        float inputX = Input.GetAxis(controller.RightHorizontal);
        float inputY = Input.GetAxis(controller.RightVertical);
        if (inputX != 0)
            dir.x = inputX;
        if (inputY != 0)
            dir.y = inputY;
        AnimationDirectionSet();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }

    private void HandleMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
        AnimationDirectionSet();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }

    private void AnimationDirectionSet()
    {
        Vector2 direction = dir.normalized;
        animator.SetFloat("directionX", direction.x);
        animator.SetFloat("directionY", direction.y);
    }

    public void GetPushed(Vector2 pushForce)
    {
        if(!freezeInput)
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
        StopCoroutine(HandlePush(new Vector2(0,0)));
        StartCoroutine(HandleStun(duration));
    }

    IEnumerator HandleStun(float duration)
    {
        freezeInput = true;
        StunBlink();
        yield return new WaitForSeconds(duration);
        freezeInput = false;
    }

    private async void StunBlink()
    {
        while (freezeInput)
        {
            await Task.Delay(100);
            spriteRenderer.color = spriteRenderer.color == originalColor ? Color.white : originalColor;
        }
        spriteRenderer.color = originalColor;
    }

    // use in animation where you want to kill the velocity of the object.
    public void KillVelocity()
    {
        rb2d.velocity = Vector2.zero;
    }

    public void ResetMovement()
    {
        StopAllCoroutines();
        spriteRenderer.color = originalColor;
        canJump = true;
        freezeInput = false;
    }
}