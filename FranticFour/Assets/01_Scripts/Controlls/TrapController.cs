using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [Tooltip("The points that the trap will move inbetween")]
    [SerializeField] Transform[] patrolPoints = new Transform[0];
    [Tooltip("The speed that the trap moves (needs to be low numbers")]
    [SerializeField] float movementSpeed = 5f;
    [Tooltip("The speed that the trap rotates at (don't forget to increase linear drag on the Rigidbody")]
    [SerializeField] float rotationSpeed = 10f;

    [Header("Timer configuration")]
    [Tooltip("The range in seconds until the trap activates when deactivated (X is lowest, Y is highest (inclusive)")]
    [SerializeField] Vector2 activationTimer = new Vector2(2,10);
    [Tooltip("The range in seconds until the trap deactivates when activated (X is lowest, Y is highest (inclusive)")]
    [SerializeField] Vector2 deactivationTimer = new Vector2(2,10);

    [Header("Other")]
    [Tooltip("Place the gameobject carrying the collider here as a child to the trap, to turn it on or off on the timer")]
    [SerializeField] GameObject childCollider = null; // this might be removed once we get animations in place for timed traps properly

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
    Vector3 targetPosition;
    int patrolPointIndex = 0;
    bool increasePoints = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartTrap();
    }
    private void StartTrap()
    {
        transform.position = patrolPoints[0].transform.position;
        targetPosition = patrolPoints[0].transform.position;
        if (timed)
        {
            int random = Random.Range(0, 2);
            if(random == 0)
                StartCoroutine(ActivateTrap());
            else
                StartCoroutine(DeactivateTrap());
        }
    }

    void FixedUpdate()
    {
        if (moving)
            MoveTrap();
        if (rotating)
            rb2d.AddTorque(rotationSpeed);
        else
            rb2d.SetRotation(0);
    }
    private void MoveTrap()
    {
        ChangeTrapPosition();
        if (transform.position == targetPosition)
            targetPosition = GetNewTargetPosition();
    }
    private void ChangeTrapPosition()
    {
        float movementThisUpdate = movementSpeed * Time.fixedDeltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisUpdate);
    }
    private Vector3 GetNewTargetPosition()
    {
        ChangePatrolIndex();
        if (backtrack)
            CheckBacktrackPatrolIndex();
        else
            CheckLoopingPatrolIndex();
        return patrolPoints[patrolPointIndex].transform.position;
    }

    private void ChangePatrolIndex()
    {
        if (increasePoints)
            patrolPointIndex++;
        else
            patrolPointIndex--;
    }
    private void CheckBacktrackPatrolIndex()
    {
        if (patrolPointIndex >= patrolPoints.Length - 1)
            ChangePatrolIndex(patrolPoints.Length - 1, false);
        if (patrolPointIndex <= 0)
            ChangePatrolIndex(0, true);
    }
    private void CheckLoopingPatrolIndex()
    {
        if (patrolPointIndex >= patrolPoints.Length || patrolPointIndex < 0)
            ChangePatrolIndex(0, true);
    }

    private void ChangePatrolIndex(int index, bool increase)
    {
        increasePoints = increase;
        patrolPointIndex = index;
    }

    IEnumerator ActivateTrap()
    {
        TrapActive(true);
        yield return new WaitForSeconds(Random.Range(activationTimer.x, activationTimer.y));
        StartCoroutine(DeactivateTrap());
    }
    IEnumerator DeactivateTrap()
    {
        TrapActive(false);
        yield return new WaitForSeconds(Random.Range(deactivationTimer.x, deactivationTimer.y));
        StartCoroutine(ActivateTrap());
    }
    private void TrapActive(bool active)
    {
        // the animation for the traps on off will be set here (using the bool active passed in)4
        print("trap active is " + active); // remove this once everything is in place
        childCollider.SetActive(active); // this will be controlled in the animation 
    }
}
