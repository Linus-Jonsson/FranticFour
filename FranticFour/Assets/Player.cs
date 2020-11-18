using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCD = 2f;



    PushController pushController;
    MovementController movController;

    bool canPush = true;


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
    }

    IEnumerator PushOtherPlayer()
    {
        canPush = false;
        pushController.PushTarget(movController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCD);
        canPush = true;
    }


}
