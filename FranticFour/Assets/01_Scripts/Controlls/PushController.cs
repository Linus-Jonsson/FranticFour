using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PushController : MonoBehaviour
{
    [SerializeField] List<Transform> targets = new List<Transform>();
    Player player;
    Rigidbody2D rb2d;
    Animator animator;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        player = GetComponentInParent<Player>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

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
        PushController closestTarget = GetClosestTarget();
        closestTarget.GetPushed(pushForce,GetComponentInParent<Player>());
    }

    private PushController GetClosestTarget()
    {
        Transform closestTarget = targets[0];
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

        return closestTarget.GetComponentInChildren<PushController>();
    }

    public bool InPushRange()
    {
        return targets.Count > 0;
    }

    public void GetPushed(Vector2 pushForce, Player pusher)
    {
        if (!player.FreezeInput)
            HandlePush(pushForce, pusher);
    }

    private void HandlePush(Vector2 pushForce, Player pusher)
    {
        player.FreezeInput = true;
        animator.SetTrigger("Pushed");
        player.PushedBy = pusher;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
    }

    public void EndPush()
    {
        player.FreezeInput = false;
        player.PushedBy = null;
    }

    public void ResetPushList()
    {
        targets = new List<Transform>();
    }

    public void RemoveFromPushList(Transform transform)
    {

    }
}