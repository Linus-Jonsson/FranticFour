using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;


    Vector2 dir = new Vector2(0, 0);
    public Vector2 Dir { get { return dir; }}
    Rigidbody2D rb2d;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MovePlayer();
        HandleRotation();
    }

    private void MovePlayer()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb2d.AddForce(movement * movementSpeed);
    }

    private void HandleRotation()
    {
        // add a small check if controller is assigned or not and use an if statement to controll what rotation to use.
        HandleControllerRotation();
        HandleMouseRotation();
    }

    private void HandleControllerRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
            dir.x = Input.GetAxis("Mouse X");
        if (Input.GetAxis("Mouse Y") != 0)
            dir.y = Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
    private void HandleMouseRotation()
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
}
