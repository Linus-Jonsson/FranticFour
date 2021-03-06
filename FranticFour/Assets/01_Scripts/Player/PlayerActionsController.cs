﻿using System.Collections;
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
    
    [Header("Push configuration")]
    [Tooltip("The amount that the players current velocity gets multiplied by at the start")]
    [SerializeField] float pushForceMultiplier = 5.0f;

    [Header("Jump configuration")]
    [Tooltip("The drag on the rigidBody while jumping (This should be low due to no force applied during the jump")]
    [SerializeField] float jumpingDrag = 0.3f;
    [SerializeField] private GameObject jumpParticles;

    [Header("Prey Configuration")]
    [SerializeField] PreyTrap preyTrap = null;

    [SerializeField] float rawOffset = 1f;
    [SerializeField] float trapPushForce = 2f;
    [SerializeField] float trapsCoolDown = 4f;

    public float TrapsCoolDown => trapsCoolDown;

    [Header("Hunter Configuration")]
    [SerializeField] float pushForce = 10f;

    [SerializeField] float pushCooldown = 2f;

    public float PushCooldown => pushCooldown;
    public UnityEvent OnPush = new UnityEvent();
    public UnityEvent OnTrapThrow = new UnityEvent();

    float originalDrag;

    [SerializeField] List<PreyTrap> laidTraps = new List<PreyTrap>();

    bool canThrowTraps = true;
    bool canJump = true;

    Player player;
    public Player Player => player;

    PushController pushController;
    RotationController rotationController;
    Animator animator;
    AssignedController controller;
    Rigidbody2D rb2d;
    PlayerAudio playerAudio;

    private void Awake()
    {
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
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        if (player.FreezeInput || player.Dead || PauseMenu.IsGamePaused)
            return;

        if (controller.UsesMouse)
            HandleMouseInput();
        else if (Input.GetAxis(controller.Action1) > DeadZones.DEADZONE_TRIGGER)
            HandlePushOrThrow();

        if (canJump && Input.GetButton(controller.Jump))
            animator.SetTrigger("Jump");
    }

    private void HandleMouseInput()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (player.Prey && canThrowTraps)
            StartCoroutine(HandleTrapThrow());
        else if (!player.Prey && player.CanPush)
            HandlePush();
    }

    private void HandlePushOrThrow()
    {
        laidTraps.RemoveAll(trap => trap == null);

        if (player.Prey && canThrowTraps)
            StartCoroutine(HandleTrapThrow());
        else if (!player.Prey && player.CanPush)
            HandlePush();
    }

    IEnumerator HandleTrapThrow()
    {
        canThrowTraps = false;
        ThrowTrap();
        yield return new WaitForSeconds(trapsCoolDown);
        canThrowTraps = true;
    }

    private void ThrowTrap()
    {
        OnTrapThrow.Invoke();
        Vector2 direction = rotationController.Dir.normalized;
        Vector3 offset = direction * rawOffset;
        PreyTrap newTrap = Instantiate(preyTrap, transform.position + offset, Quaternion.identity);
        newTrap.PushTrap(direction * trapPushForce);
        laidTraps.Add(newTrap);
    }

    private void HandlePush()
    {
        // playerAudio.PlaySound("push"); - Removed temporarily(?) - not working properly (distorted sound)
        animator.SetTrigger("Push");
        if (pushController.InPushRange())
            StartCoroutine(PushOtherPlayer());
    }

    IEnumerator PushOtherPlayer()
    {
        player.CanPush = false;
        OnPush.Invoke();
        pushController.PushTarget(rotationController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCooldown);
        player.CanPush = true;
    }

    public void ResetPlayerActions()
    {
        ResetTraps();
        StopAllCoroutines();
        Jumping(false);
        canThrowTraps = true;
    }

    private void ResetTraps()
    {
        laidTraps.RemoveAll(trap => trap == null);
        foreach (var trap in laidTraps)
            trap.DestroyTrap();
    }

    public void Jumping(bool value)
    {
        canJump = !value;
        if (value)
        {
            playerAudio.PlaySound("jump");
            if (!(jumpParticles is null))
                Instantiate(jumpParticles, transform.position, Quaternion.identity);
            else
                Debug.LogError("Missing jump particle effect.");

            SetLayerAndDrag(jumpLayer, jumpingDrag);
        }
        else
            SetLayerAndDrag(playerLayer, originalDrag);
    }

    private void SetLayerAndDrag(int layer, float drag)
    {
        gameObject.layer = layer;
        rb2d.drag = drag;
    }

    //AnimationEvents:
    public void StartJumping()
    {
        Jumping(true);    
    }

    public void EndJumping()
    {
        Jumping(false);
        rb2d.freezeRotation = false;
        player.FreezeInput = false;
    }
    
    public void StartPushing()
    {
        playerAudio.PlaySound("push");
        rb2d.AddForce(rotationController.Dir * pushForceMultiplier, ForceMode2D.Impulse);
        player.FreezeInput = true;
    }

    public void EndPushing()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.freezeRotation = false;
        player.FreezeInput = false;
    }
}
