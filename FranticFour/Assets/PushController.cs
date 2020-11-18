using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    List<Transform> targets = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            targets.Add(other.gameObject.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            targets.Remove(other.gameObject.transform);
    }

    public void PushTarget(Vector2 pushForce)
    {
        if (targets.Count == 0)
            return;
        MovementController closestTarget = GetClosestTarget();
        closestTarget.GetPushed(pushForce);

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
}
