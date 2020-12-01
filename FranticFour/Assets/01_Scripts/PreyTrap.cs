using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTrapTriggeredAnimation();
            SetPlayerStunAnimation(other.GetComponent<Player>());
        }
    }

    private void SetTrapTriggeredAnimation()
    {
        animator.SetTrigger(""); // name this to the trigger that starts the trap triggered animaton
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void SetPlayerStunAnimation(Player player)
    {
        player.FreezeInput = true;
        player.GetComponent<Animator>().SetTrigger("");// name this to the trigger that starts the player stun animation
    }

    public void DestroyTrap()
    {
        Destroy(gameObject);
    }

    public void PushTrap(Vector2 pushForce)
    {
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
    }
}