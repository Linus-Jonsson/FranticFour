using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    [SerializeField] float stunDelay = 2;

    Rigidbody2D rb2d;
    PlayerActionsController playerActionsController;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<MovementController>().GetStunned(stunDelay);
            DestroyTrap(); // remove this once animation destroys the trap.
        }
    }

    public void DestroyTrap()
    {
        playerActionsController.DecreaseThrownTraps();
        Destroy(gameObject);
    }

    public void PushTrap(Vector2 pushForce)
    {
        rb2d.AddForce(pushForce,ForceMode2D.Impulse);
    }

    public void SetPlayerActionController(PlayerActionsController playerActionsController)
    {
        this.playerActionsController = playerActionsController;
    }

}