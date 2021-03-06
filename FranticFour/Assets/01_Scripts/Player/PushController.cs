﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class PushController : MonoBehaviour
{
    [SerializeField] List<Player> targets = new List<Player>();
    [Tooltip("The time (in milliseconds) that the game freezes when getting pushed")]
    [SerializeField] int freezeDuration = 150;

    float pushRange = 0f;

    Player player;
    Rigidbody2D rb2d;
    Animator animator;
    PlayerAudio playerAudio;
    PlayerActionsController playerActionsController;


    private void Awake()
    {
        GetReferences();
        float x = GetComponent<BoxCollider2D>().size.x *2;
        float y = GetComponent<BoxCollider2D>().size.y *2;
        pushRange = Mathf.Sqrt(x + y);
    }

    private void GetReferences()
    {
        player = GetComponentInParent<Player>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        playerAudio = GetComponentInParent<PlayerAudio>();
        playerActionsController = GetComponentInParent<PlayerActionsController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (!other.GetComponentInParent<Player>().Dead)
                targets.Add(other.GetComponentInParent<Player>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // the try loop is just a extremely quick solution to an error, find a more 
        // elegant and better way to fix the null reference error (that doesnt come often)
        if (other.gameObject.CompareTag("Player"))
            try
            {
                if (!other.GetComponentInParent<Player>().Dead)
                    targets.Remove(other.GetComponentInParent<Player>());
            }
            catch
            {
                Debug.LogWarning("Trying to remove target when there is no target");
            }
    }

    public void PushTarget(Vector2 pushForce)
    {
        PushController closestTarget = GetClosestTarget();
        if(closestTarget != null)
        closestTarget.GetPushed(pushForce,GetComponentInParent<Player>());
    }

    private PushController GetClosestTarget()
    {
        Transform closestTarget = targets[0].transform;
        Transform player = gameObject.transform;
        float closestTargetDistance = Vector2.Distance(closestTarget.position, player.position);

        foreach (var target in targets)
        {
            if (target.Prey)
                return target.GetComponentInChildren<PushController>();

            float targetDistance = Vector2.Distance(target.transform.position, player.position);
            if (targetDistance < closestTargetDistance)
            {
                closestTarget = target.transform;
                closestTargetDistance = Vector2.Distance(closestTarget.position, player.position);
            }
        }

        if (closestTargetDistance > pushRange)
        {
            Debug.LogWarning(closestTarget.name + " Is to far away to be pushed");
            return null;
        }

        return closestTarget.GetComponentInChildren<PushController>();
    }

    public bool InPushRange()
    {
        targets.RemoveAll(target => target == null);
        return targets.Count > 0;
    }

    public void GetPushed(Vector2 pushForce, Player pusher)
    {
        HandlePush(pushForce, pusher);
    }

    private void HandlePush(Vector2 pushForce, Player pusher)
    {
        player.ResetPlayerColor();
        playerActionsController.Jumping(false);
        playerAudio.PlaySound("pushed");
        Thread.Sleep(freezeDuration);
        player.FreezeInput = true;
        animator.SetTrigger("Pushed");

        SetPusherAndAssistPusher(pusher);

        player.RestartAssistCounter();
        rb2d.velocity = Vector2.zero;
        if (!player.IsPushed)
            AddPushForce(pushForce);
    }

    private void SetPusherAndAssistPusher(Player pusher)
    {
        if (player.AssistPusher != null)
            if (pusher.name == player.AssistPusher.name)
                player.AssistPusher = null;

        if (player.PushedBy != null)
            if (player.PushedBy.name != pusher.name)
                player.AssistPusher = player.PushedBy;

        player.PushedBy = pusher;
    }

    private async void AddPushForce(Vector2 pushForce)
    {        
        player.IsPushed = true;
        while(player.IsPushed)
        {            
            rb2d.AddForce(pushForce);
            await Task.Delay(10);
        }
    }

    public void ResetPushList()
    {
        targets = new List<Player>();
    }
}