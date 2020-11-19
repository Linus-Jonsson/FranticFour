using System.Collections;
using System.Collections.Generic;
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


    [Header("Jump configuration")]
    [Tooltip("The duration of the jump in seconds")]
    [SerializeField] float jumpDuration = 2f;
    [Tooltip("The cooldown on jumping in seconds (starts to count after jump is finished)")]
    [SerializeField] float jumpCooldown = 2f;
    [Tooltip("The drag on the rigidBody while jumping (This should be low due to no force applied during the jump")]
    [SerializeField] float jumpingDrag = 0.3f;



    Vector2 dir = new Vector2(0, 0);

    public Vector2 Dir { get { return dir; } }

    Rigidbody2D rb2d;

    bool freezeInput = false;
    public bool FreezeInput { get { return freezeInput; } }


    bool canJump = true;

    float originalDrag;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        originalDrag = rb2d.drag;
    }

    private void Update()
    {
        if (canJump && Input.GetButton("Jump") && !freezeInput)
        {
            StartCoroutine(HandleJump());
        }
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
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical")).normalized;
        rb2d.AddForce(movement * movementSpeed);
    }

    IEnumerator HandleJump()
    {
        StartJumping();
        yield return new WaitForSeconds(jumpDuration);
        EndJumping();
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
    private void StartJumping()
    {
        canJump = false;
        gameObject.layer = jumpLayer;
        rb2d.drag = jumpingDrag;
        freezeInput = true;
    }
    private void EndJumping()
    {
        gameObject.layer = playerLayer;
        rb2d.drag = originalDrag;
        freezeInput = false;
    }


    private void HandleRotation()
    {
        // add a small check if controller is assigned or not and use an if statement to controll what rotation to use.
        HandleControllerRotation();
        HandleMouseRotation();
    }

    private void HandleControllerRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
            dir.x = Input.GetAxis("Mouse X");
        if (Input.GetAxis("Mouse Y") != 0)
            dir.y = Input.GetAxis("Mouse Y");

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
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
    }

    public void GetStunned(float duration)
    {
        StartCoroutine(HandleStun(duration));
    }

    IEnumerator HandleStun(float duration)
    {
        freezeInput = true;
        yield return new WaitForSeconds(duration);
        freezeInput = false;
    }
}
