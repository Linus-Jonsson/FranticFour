﻿using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    List<Transform> targets = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            targets.Add(other.gameObject.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            targets.Remove(other.gameObject.transform);
    }

    public void PushTarget(Vector2 pushForce)
    {
        if (targets.Count == 0)
            return;
        MovementController closestTarget = GetClosestTarget();
        closestTarget.GetPushed(pushForce);
        closestTarget.GetComponent<Player>().GetPushedBy(GetComponentInParent<Player>());
    }

    private MovementController GetClosestTarget()
    {
        var closestTarget = targets[0];
        Transform player = gameObject.transform;
        float closestTargetDistance = Vector2.Distance(closestTarget.position, player.position);

        foreach (var target in targets)
        {
            float targetDistance = Vector2.Distance(target.position, player.position);
            if (targetDistance < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = Vector2.Distance(closestTarget.position, player.position);
            }
        }

        return closestTarget.GetComponent<MovementController>();
    }

    public bool InPushRange()
    {
        return targets.Count > 0;
    }
}