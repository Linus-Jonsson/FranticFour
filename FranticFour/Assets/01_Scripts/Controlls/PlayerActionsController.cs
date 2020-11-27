using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionsController : MonoBehaviour
{
    [Header("Layer configuration")]
    [Tooltip("Set this to have the layerNumber of the layer that player is")]
    [SerializeField] int playerLayer = 8;
    [Tooltip("Set this to have the layerNumber of the layer that Jump is")]
    [SerializeField] int jumpLayer = 9;

    [Header("Jump configuration")]
    [Tooltip("The drag on the rigidBody while jumping (This should be low due to no force applied during the jump")]
    [SerializeField] float jumpingDrag = 0.3f;

    [Header("Prey Configuration")]
    [SerializeField] PreyTrap preyTrap = null;
    [SerializeField] float rawOffset = 1f;
    [SerializeField] float trapPushForce = 2f;
    [SerializeField] float trapsCoolDown = 4f;
    [SerializeField] int maximumTraps = 5;

    [Header("Hunter Configuration")]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCooldown = 2f;

    public float PushCooldown => pushCooldown;
    public UnityEvent OnPush = new UnityEvent();

    int thrownTraps = 0;
    float originalDrag;

    bool canThrowTraps = true;
    bool canPush = true;
    [SerializeField] bool canJump = true;
    bool prey;

    PushController pushController;
    RotationController rotationController;
    Player player;
    Animator animator;    
    AssignedController controller;
    Rigidbody2D rb2d;
    List<PreyTrap> laidTraps = new List<PreyTrap>();

    private void Start()
    {
        canJump = true;
        GetReferences();
        originalDrag = rb2d.drag;
    }

    private void GetReferences()
    {
        controller = GetComponent<AssignedController>();
        pushController = GetComponentInChildren<PushController>();
        rotationController = GetComponent<RotationController>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        int m_number;
        bool mouseUsed = Int32.TryParse(controller.Action1, out m_number);

        if (canJump && Input.GetButton(controller.Jump) && !player.FreezeInput)
            animator.SetTrigger("Jump");

        if (!Input.GetButtonDown(controller.Action1) &&
            !(Input.GetAxis(controller.Action1) > 0) ||
            player.FreezeInput)
            return;
        
        if (mouseUsed)
        {
            if (!Input.GetMouseButton(m_number))
                return;
            if (player.Prey && canThrowTraps && thrownTraps < maximumTraps)
                StartCoroutine(ThrowTrap());
            else if (!player.Prey && canPush && pushController.InPushRange())
                HandlePush();
        }

        if (player.Prey && canThrowTraps && thrownTraps < maximumTraps)
            StartCoroutine(ThrowTrap());
        else if (!player.Prey && canPush)
            HandlePush();
    }

    IEnumerator ThrowTrap()
    {
        thrownTraps++;
        canThrowTraps = false;
        Vector2 direction = rotationController.Dir.normalized;
        Vector3 offset = direction * rawOffset;
        PreyTrap newTrap = Instantiate(preyTrap, transform.position + offset, Quaternion.identity);
        newTrap.PushTrap(direction * trapPushForce);
        newTrap.SetPlayerActionController(this);
        laidTraps.Add(newTrap);
        yield return new WaitForSeconds(trapsCoolDown);
        canThrowTraps = true;
    }

    private void HandlePush()
    {
        animator.SetTrigger("Push");
        if (pushController.InPushRange())
            StartCoroutine(PushOtherPlayer());
    }
    IEnumerator PushOtherPlayer()
    {
        OnPush.Invoke();
        canPush = false;
        pushController.PushTarget(rotationController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCooldown);
        canPush = true;
    }

    public void DecreaseThrownTraps()
    {
        thrownTraps--;
        if (thrownTraps < 0)
            thrownTraps = 0;
    }

    private void ResetTraps()
    {
        laidTraps.RemoveAll(trap => trap == null);
        foreach (var trap in laidTraps)
        {
            trap.DestroyTrap();
        }
    }

    public void ResetPlayerActions()
    {
        ResetTraps();
        StopAllCoroutines();
        canPush = true;
        canThrowTraps = true;
    }

    //AnimationEvents:
    public void StartJumping()
    {
        canJump = false;
        rb2d.freezeRotation = true;
        gameObject.layer = jumpLayer;
        rb2d.drag = jumpingDrag;
        player.FreezeInput = true;
    }
    public void EndJumping()
    {
        rb2d.freezeRotation = false;
        gameObject.layer = playerLayer;
        rb2d.drag = originalDrag;
        player.FreezeInput = false;
        canJump = true;
    }
}
