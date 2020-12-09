﻿using System;
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

    public float TrapsCoolDown => trapsCoolDown;

    [SerializeField] int maximumTraps = 5;

    [Header("Hunter Configuration")]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCooldown = 2f;

    public float PushCooldown => pushCooldown;
    public UnityEvent OnPush = new UnityEvent();
    public UnityEvent OnTrapThrow = new UnityEvent();

    float originalDrag;

    List<PreyTrap> laidTraps = new List<PreyTrap>();

    bool canThrowTraps = true;
    bool canPush = true;
    bool canJump = true;

    Player player;
    public Player Player => player;

    PushController pushController;
    RotationController rotationController;
    Animator animator;    
    AssignedController controller;
    Rigidbody2D rb2d;


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
    }

    private void Update()
    {
        if (player.FreezeInput || player.Dead || PauseMenu.IsGamePaused)
            return;
        int m_number;
        bool mouseUsed = Int32.TryParse(controller.Action1, out m_number);
        if (mouseUsed)
            HandleMouseInput(m_number);
        if (canJump && Input.GetButton(controller.Jump))
            animator.SetTrigger("Jump");
        if (Input.GetAxis(controller.Action1) > 0 || Input.GetButtonDown(controller.Action1))
            HandlePushOrThrow();
    }
    private void HandleMouseInput(int m_number)
    {
        if (!Input.GetMouseButton(m_number))
            return;
        if (player.Prey && canThrowTraps && laidTraps.Count < maximumTraps)
            StartCoroutine(HandleTrapThrow());
        else if (!player.Prey && canPush)
            HandlePush();
    }
    private void HandlePushOrThrow()
    {
        laidTraps.RemoveAll(trap => trap == null);
        if (player.Prey && canThrowTraps && laidTraps.Count < maximumTraps)
            StartCoroutine(HandleTrapThrow());
        else if (!player.Prey && canPush)
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

    public void ResetPlayerActions()
    {
        ResetTraps();
        StopAllCoroutines();
        Jumping(false);
        canPush = true;
        canThrowTraps = true;
    }
    private void ResetTraps()
    {
        laidTraps.RemoveAll(trap => trap == null);
        foreach (var trap in laidTraps)
            trap.DestroyTrap();
    }

    private void Jumping(bool value)
    {
        canJump = !value;
        rb2d.freezeRotation = value;
        player.FreezeInput = value;
        if (value)
            SetLayerAndDrag(jumpLayer,jumpingDrag);
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
    }
}
