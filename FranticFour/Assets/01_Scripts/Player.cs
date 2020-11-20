using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCooldown = 2f;
    public float PushCooldown => pushCooldown;
    public UnityEvent OnPush = new UnityEvent();
    [SerializeField] public AssignedController controller;
    [SerializeField] private GameObject deathParticles = null;
    
    PushController pushController;
    MovementController movementController;

    [SerializeField] bool canPush = true;

    [SerializeField] bool prey = false;

    void Start()
    {
        controller = GetComponent<AssignedController>();
        pushController = GetComponentInChildren<PushController>();
        movementController = GetComponent<MovementController>();
        transform.position = new Vector3(Random.Range(-7f,1f),Random.Range(-0.38f, 4.3f), transform.position.z);
    }

    private void Update()
    {
        if(prey) { return; }
        if (canPush && !movementController.FreezeInput && pushController.InPushRange() &&
            (Input.GetButtonDown(controller.Action1) || Input.GetAxis(controller.Action1) > 0))//Hämta input
        {
            StartCoroutine(PushOtherPlayer());
        }
    }

    IEnumerator PushOtherPlayer()
    {
        OnPush.Invoke();
        canPush = false;
        pushController.PushTarget(movementController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCooldown);
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
        if (deathParticles) //Null check
            Instantiate(deathParticles, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        transform.position = new Vector3(Random.Range(-7f,1f),Random.Range(-0.38f, 4.3f), transform.position.z);
    }
}
