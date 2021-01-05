using System.Collections;
using UnityEngine;

public class PreyTrap : MonoBehaviour
{
    [Tooltip("After this time the trap self destructs")]
    [SerializeField] float selfDestructTimer = 10f;

    Rigidbody2D rb2d;
    Animator animator;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(trapCountDown());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().Dead)
                return;
            SetTrapTriggeredAnimation();
            SetPlayerStunAnimation(other.GetComponent<Player>());
        }
        if(other.CompareTag("Danger"))
        {
            DestroyTrap();
        }
    }

    private void SetTrapTriggeredAnimation()
    {
        rb2d.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("Triggered");
    }

    private void SetPlayerStunAnimation(Player player)
    {
        GetComponent<AudioSource>().Play();
        player.GetComponent<AfterImageController>().ResetAfterImage();
        player.IsPushed = false;
        player.FreezeInput = true;
        player.GetComponent<Animator>().SetTrigger("Stunned");
    }

    public void DestroyTrap()
    {
        Destroy(gameObject);
    }

    public void PushTrap(Vector2 pushForce)
    {
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
    }

    IEnumerator trapCountDown()
    {
        while(selfDestructTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            selfDestructTimer -= 1f;
        }
        DestroyTrap();
    }
}