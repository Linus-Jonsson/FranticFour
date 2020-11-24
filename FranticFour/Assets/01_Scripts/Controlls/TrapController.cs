using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [Tooltip("The points that the trap will move inbetween")]
    [SerializeField] Transform[] patrolPoints = new Transform[0];
    [Tooltip("The speed that the trap moves (needs to be low numbers")]
    [SerializeField] float movementSpeed = 10f;
    [Tooltip("The speed that the trap rotates at")]
    [SerializeField] float rotationSpeed = 10f;

    [Header("Timer configuration")]
    [Tooltip("The range in seconds until the trap activates when deactivated (X is lowest, Y is highest (inclusive)")]
    [SerializeField] Vector2 activationTimer = new Vector2(2,10);
    [Tooltip("The range in seconds until the trap deactivates when activated (X is lowest, Y is highest (inclusive)")]
    [SerializeField] Vector2 deactivationTimer = new Vector2(2,10);

    [Header("Booleans for determining the traps behaviour")]
    [Tooltip("Determines if the trap should move along the partol points (Don't forget to add them)")]
    [SerializeField] bool moving = false;
    [Tooltip("Determines if the trap should backtrack its path or go to the first patrol point once it has reached the last one")]
    [SerializeField] bool backtrack = false;
    [Tooltip("Determines if the trap should have a timer function for on and off")]
    [SerializeField] bool timed = false;
    [Tooltip("Determines if the trap should rotate")]
    [SerializeField] bool rotating = false;

    Rigidbody2D rb2d;
    GameObject childCollider;

    int patrolPointIndex = 0;
    bool increasePoints = true;

    bool active = false;
    bool trapActivated = false;


    void Start()
    {
        GetReferences();
        StartTrap();
    }

    private void GetReferences()
    {
        rb2d = GetComponent<Rigidbody2D>();
        childCollider = GetComponentInChildren<GameObject>();
    }

    private void StartTrap()
    {
        transform.position = patrolPoints[patrolPointIndex].transform.position;
        if (timed)
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                childCollider.SetActive(false);
                StartCoroutine(ActivateTrap());
            }
            else
            {
                childCollider.SetActive(true);
                StartCoroutine(DeactivateTrap());
            }
        }
    }

    void FixedUpdate()
    {
        if(moving)
        {
            MoveTrap();
        }
        if (rotating)
        {
            rb2d.AddTorque(rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb2d.SetRotation(0);
        }

        if(active && !trapActivated)
        {
            trapActivated = true;
            childCollider.SetActive(true); // this will be controlled in the animation 
        }
        else if (!active && trapActivated)
        {
            trapActivated = false;
            childCollider.SetActive(false); // this will be controlled in the animation 
        }
    }

    private void MoveTrap()
    {
        Vector3 targetPosition = patrolPoints[patrolPointIndex].transform.position;
        float movementThisUpdate = movementSpeed * Time.fixedDeltaTime;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisUpdate);

        if (transform.position == targetPosition)
        {
            if (backtrack)
            {
                ChangeBacktrackPatrolIndex();
            }
            else
            {
                ChangePatrolIndex();
            }
        }
    }
    private void ChangeBacktrackPatrolIndex()
    {
        if (increasePoints)
        {
            patrolPointIndex++;
            if (patrolPointIndex >= patrolPoints.Length - 1)
            {
                patrolPointIndex = patrolPoints.Length - 1;
                increasePoints = !increasePoints;
            }
        }
        else
        {
            patrolPointIndex--;
            if (patrolPointIndex <= 0)
            {
                patrolPointIndex = 0;
                increasePoints = !increasePoints;
            }
        }
    }
    private void ChangePatrolIndex()
    {
        patrolPointIndex++;
        if (patrolPointIndex >= patrolPoints.Length)
        {
            patrolPointIndex = 0;
        }
    }


    IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(Random.Range(activationTimer.x, activationTimer.y));
        active = true;
        StartCoroutine(DeactivateTrap());
    }
    IEnumerator DeactivateTrap()
    {
        yield return new WaitForSeconds(Random.Range(deactivationTimer.x, deactivationTimer.y));
        active = false;
        StartCoroutine(ActivateTrap());
    }
}
