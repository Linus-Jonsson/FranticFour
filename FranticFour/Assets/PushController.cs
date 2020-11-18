using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    MovementController player;
    [SerializeField] List<Transform> targets = new List<Transform>();


    private void Start()
    {
        player = GetComponentInParent<MovementController>();
    }


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

    public MovementController GetTargetToPush()
    {
        if(targets.Count == 0)
            return null;

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
