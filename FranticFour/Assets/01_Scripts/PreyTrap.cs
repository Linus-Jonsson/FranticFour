using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    [SerializeField] private float stunDelay = 2;
    private GameObject player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.gameObject;
        StartCoroutine(StunPlayer(player));
    }

    private IEnumerator StunPlayer(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().Sleep();
        player.GetComponent<MovementController>().enabled = false;
        //create electric effect + player shake
        yield return new WaitForSeconds(stunDelay);
        player.GetComponent<Rigidbody2D>().WakeUp();
        player.GetComponent<MovementController>().enabled = true;
        //Destroy Effect
        Destroy(gameObject);
    }
}
