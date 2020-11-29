using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedWalker : MonoBehaviour
{
    [SerializeField] float xMovSpeed = 10f;
    [SerializeField] float yMovSpeed = 0f;
    [SerializeField] float walkTimer = 5f;
    [SerializeField] bool turnAround = false;

    [SerializeField] float timer = 0;

    Rigidbody2D rb2d;
    void Start()
    {
        timer = walkTimer;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            timer = walkTimer;
            turnAround = !turnAround;
        }

        float newX = transform.position.x + xMovSpeed;
        float newY = transform.position.y + yMovSpeed;
        if (!turnAround)
        {
            rb2d.AddForce(new Vector2(newX,newY));
        }
        else
        {
            rb2d.AddForce(new Vector2(-newX,-newY));
        }
    }
}
