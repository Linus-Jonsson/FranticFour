using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushCD = 2f;

    Vector2 dir = new Vector2(0, 0);

    Rigidbody2D rb2d;
    PushController pushController;

    bool canPush = true;

    void Start()
    {
        pushController = GetComponentInChildren<PushController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canPush && (Input.GetButtonDown("Push") || Input.GetAxis("Push") > 0))
        {
            PushOtherPlayer();
        }
    }

    private void PushOtherPlayer()
    {
        canPush = false;
        var target = pushController.GetTargetToPush();
        if (target != null)
            target.GetPushed(dir.normalized * pushForce);
        StartCoroutine(ResetPush());
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb2d.AddForce(movement * movementSpeed);
    }

    private void RotatePlayer()
    {
        SetControllerRotation();
        //SetMouseRotation();
    }
    private void SetControllerRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
            dir.x = Input.GetAxis("Mouse X");
        if (Input.GetAxis("Mouse Y") != 0)
            dir.y = Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
    private void SetMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }


    public void GetPushed(Vector2 pushForce)
    {
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
    }

    IEnumerator ResetPush()
    {
        yield return new WaitForSeconds(pushCD);
        canPush = true;
    }
}
