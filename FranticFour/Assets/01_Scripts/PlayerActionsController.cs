using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionsController : MonoBehaviour
{    
    [SerializeField] AssignedController controller; // does this need to have SerializeField since we get it in start?

    [Header("Prey Configuration")]
    [SerializeField] GameObject preyTrap = null;
    [SerializeField] float throwDistance = 1f;
    [SerializeField] float trapsCoolDown = 4f;

    [Header("Hunter Configuration")]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCooldown = 2f;

    public float PushCooldown => pushCooldown;
    public UnityEvent OnPush = new UnityEvent();

    bool canThrowTraps = true;
    bool canPush = true;
    bool prey;

    PushController pushController;
    MovementController movementController;
    Player player;

    private void Start()
    {
        controller = GetComponent<AssignedController>();
        pushController = GetComponentInChildren<PushController>();
        movementController = GetComponent<MovementController>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if ((Input.GetButtonDown(controller.Action1) || Input.GetAxis(controller.Action1) > 0) && 
            !movementController.FreezeInput)
        {
            if (player.Prey && canThrowTraps)
                StartCoroutine(ThrowTrap());
            else if (!player.Prey && canPush && pushController.InPushRange())
                StartCoroutine(PushOtherPlayer());
        }
    }

    IEnumerator ThrowTrap()
    {
        canThrowTraps = false;
        Vector3 offset = movementController.Dir.normalized * throwDistance;
        Instantiate(preyTrap, transform.position + offset, Quaternion.identity);
        yield return new WaitForSeconds(trapsCoolDown);
        canThrowTraps = true;
    }

    IEnumerator PushOtherPlayer()
    {
        OnPush.Invoke();
        canPush = false;
        pushController.PushTarget(movementController.Dir.normalized * pushForce);
        yield return new WaitForSeconds(pushCooldown);
        canPush = true;
    }
}
