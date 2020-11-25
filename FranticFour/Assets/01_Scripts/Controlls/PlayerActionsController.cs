using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionsController : MonoBehaviour
{    
    [SerializeField] AssignedController controller; // does this need to have SerializeField since we get it in start?

    [Header("Prey Configuration")]
    [SerializeField] PreyTrap preyTrap = null;
    [SerializeField] float rawOffset = 1f;
    [SerializeField] float trapPushForce = 2f;
    [SerializeField] float trapsCoolDown = 4f;
    [SerializeField] int maximumTraps = 5;
    [SerializeField] int thrownTraps = 0; // remove serializeField

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
    private Animator animator;

    List<PreyTrap> laidTraps = new List<PreyTrap>();

    private void Start()
    {
        controller = GetComponent<AssignedController>();
        pushController = GetComponentInChildren<PushController>();
        movementController = GetComponent<MovementController>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        int m_number;
        bool mouseUsed = Int32.TryParse(controller.Action1, out m_number);

        if (!Input.GetButtonDown(controller.Action1) &&
            !(Input.GetAxis(controller.Action1) > 0) ||
            movementController.FreezeInput)
            return;
        
        if (mouseUsed)
        {
            if (!Input.GetMouseButton(m_number))
                return;
            if (player.Prey && canThrowTraps && thrownTraps < maximumTraps)
                StartCoroutine(ThrowTrap());
            else if (!player.Prey && canPush && pushController.InPushRange())
                StartCoroutine(PushOtherPlayer());
        }
            
        if (player.Prey && canThrowTraps && thrownTraps < maximumTraps)
            StartCoroutine(ThrowTrap());
        else if (!player.Prey && canPush)
        {
            animator.SetTrigger("Push");
            if(pushController.InPushRange())
            StartCoroutine(PushOtherPlayer());
        }
    }

    IEnumerator ThrowTrap()
    {
        thrownTraps++;
        canThrowTraps = false;
        Vector2 direction = movementController.Dir.normalized;
        Vector3 offset = direction * rawOffset;
        PreyTrap newTrap = Instantiate(preyTrap, transform.position + offset, Quaternion.identity);
        newTrap.PushTrap(direction * trapPushForce);
        newTrap.SetPlayerActionController(this);
        laidTraps.Add(newTrap);
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

    public void DecreaseThrownTraps()
    {
        thrownTraps--;
        if (thrownTraps < 0)
            thrownTraps = 0;
    }

    private void ResetTraps()
    {
        laidTraps.RemoveAll(trap => trap == null);
        foreach (var trap in laidTraps)
        {
            trap.DestroyTrap();
        }
    }

    public void ResetPlayerActions()
    {
        ResetTraps();
        StopAllCoroutines();
        canPush = true;
        canThrowTraps = true;
    }
}
