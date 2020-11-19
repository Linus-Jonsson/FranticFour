using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAbility : MonoBehaviour
{
    [SerializeField] private float throwDistance = 0.5f;
    public GameObject preyTrap;
    private MovementController movementController;

    private void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {

        if (Input.GetButtonDown("Push") || Input.GetAxis("Push") > 0)
        {
            Vector3 offset = movementController.Dir.normalized * throwDistance;
            Instantiate(preyTrap, transform.position - offset, Quaternion.identity);
        }
    }
}
