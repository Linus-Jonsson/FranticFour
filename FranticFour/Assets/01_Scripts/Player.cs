﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCD = 2f;

    PushController pushController;
    MovementController movController;

    Vector2 spawnPoint; // remove this once death is properly implemented.

    bool canPush = true;

    void Start()
    {
        spawnPoint = transform.position;
        pushController = GetComponentInChildren<PushController>();
        movController = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (canPush && (Input.GetButtonDown("Push") || Input.GetAxis("Push") > 0))
        {
            StartCoroutine(PushOtherPlayer());
        }
    }

    IEnumerator PushOtherPlayer()
    {
        canPush = false;
        pushController.PushTarget(movController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCD);
        canPush = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("triggered");
        if(other.gameObject.CompareTag("Danger"))
        {
            print("dead");
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // this will be where the player death will be handled instead of just chaning its position.
        transform.position = spawnPoint; 
    }
}
