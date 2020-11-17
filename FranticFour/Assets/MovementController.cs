using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;

    Rigidbody2D rb2d;
    Vector2 dir = new Vector2(0, 0);

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        SetMouseRotation();
        Rotate();
    }

    private void SetControllerRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
            dir.x = Input.GetAxis("Mouse X");
        if (Input.GetAxis("Mouse Y") != 0)
            dir.y = Input.GetAxis("Mouse Y");
    }
    private void SetMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
}
