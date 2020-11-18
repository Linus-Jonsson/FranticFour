using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCD = 2f;

    [SerializeField] float jumpDuration = 2f;
    [SerializeField] float jumpCD = 2f;

    PushController pushController;
    MovementController movController;

    bool canPush = true;
    bool canJump = true;

    bool jumping = false;

    void Start()
    {
        pushController = GetComponentInChildren<PushController>();
        movController = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (canPush && (Input.GetButtonDown("Push") || Input.GetAxis("Push") > 0))
        {
            StartCoroutine(PushOtherPlayer());
        }
        if (canJump && Input.GetButton("Jump"))
        {
            StartCoroutine(HandleJump());
        }
    }

    IEnumerator PushOtherPlayer()
    {
        canPush = false;
        pushController.PushTarget(movController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCD);
        canPush = true;
    }

    IEnumerator HandleJump()
    {
        print("Jumping");
        canJump = false;
        jumping = true;
        yield return new WaitForSeconds(jumpDuration);
        print("Jumping done");
        jumping = false;
        yield return new WaitForSeconds(jumpCD);
        canJump = true;
        print("Jumping reset");
    }
}
