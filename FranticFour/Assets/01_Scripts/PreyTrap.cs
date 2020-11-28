using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    [SerializeField] float stunDuration = 2;
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<PlayerFuckedController>().GetStunned(stunDuration);
            DestroyTrap(); // remove this once animation destroys the trap.
        }
    }

    public void DestroyTrap()
    {
        Destroy(gameObject);
    }

    public void PushTrap(Vector2 pushForce)
    {
        rb2d.AddForce(pushForce,ForceMode2D.Impulse);
    }
}