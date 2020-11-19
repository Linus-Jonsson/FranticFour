using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    [SerializeField] float stunDelay = 2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<MovementController>().GetStunned(stunDelay);
            Destroy(gameObject); // remove this once animation destroys the trap.
        }
    }

    public void DestroyTrap()
    {
        Destroy(gameObject);
    }
}