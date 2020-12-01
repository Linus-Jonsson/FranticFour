using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    [Range(0, 5)] [SerializeField] float bounciness = 1f;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            AddForce(other.collider.GetComponent<Rigidbody2D>().velocity);
    }

    public void AddForce(Vector2 force)
    {
        Vector3 acceleration = -force / rb2d.mass * bounciness;
        rb2d.AddForce(acceleration, ForceMode2D.Impulse);
    }
}
