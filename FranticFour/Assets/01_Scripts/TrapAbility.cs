using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAbility : MonoBehaviour
{
    [SerializeField] private float throwDistance = 0.5f;
    [SerializeField] private float trapsCoolDown = 4f;
    [SerializeField] private GameObject preyTrap = null;
    private MovementController movementController;
    private bool canThrowTraps = true;
    
    [SerializeField] public AssignedController controller;
    
    private void Start()
    {
        controller = GetComponent<AssignedController>();
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        if ((Input.GetButtonDown(controller.Action1) || Input.GetAxis(controller.Action1) > 0) && canThrowTraps)
        {
            StartCoroutine(ThrowTrap());
        }
    }

    IEnumerator ThrowTrap()
    {
        canThrowTraps = false;
        Vector3 offset = movementController.Dir.normalized * throwDistance;
        Instantiate(preyTrap, transform.position - offset, Quaternion.identity);
        yield return new WaitForSeconds(trapsCoolDown);
        canThrowTraps = true;
    }
}
