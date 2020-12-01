using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerFuckedController : MonoBehaviour
{
    [Header("Push configuration")]
    [Tooltip("The time in seconds that the player getting pushed wont be able to move")]
    [SerializeField] float pushDuration = 0.3f;
    [Tooltip("The amount of time in seconds that the one who recently pushed the player is saved before going back to null")]
    [SerializeField] float pushedByTime = 2f;

    [SerializeField] GameObject body = null;
    [SerializeField] Color originalColor = new Color(0, 0, 0, 255);

    Rigidbody2D rb2d;
    Player player;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void GetPushed(Vector2 pushForce, Player pusher)
    {
        if (!player.FreezeInput)
        {
            StartCoroutine(HandlePush(pushForce));
            StopCoroutine(HandlePushedByTimer(pusher));
            StartCoroutine(HandlePushedByTimer(pusher));
        }

    }
    IEnumerator HandlePush(Vector2 pushForce)
    {
        animator.SetTrigger("Pushed");
        player.FreezeInput = true;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(pushDuration);
        player.FreezeInput = false;
    }
    IEnumerator HandlePushedByTimer(Player pusher)
    {
        player.PushedBy = pusher;
        yield return new WaitForSeconds(pushedByTime);
        player.PushedBy = null;
    }

    public void GetStunned(float duration)
    {
        StopCoroutine(HandlePush(new Vector2(0, 0)));
        StartCoroutine(HandleStun(duration));
    }
    IEnumerator HandleStun(float duration)
    {
        player.FreezeInput = true;
        StunBlink();
        yield return new WaitForSeconds(duration);
        player.FreezeInput = false;
    }

    public async void StunBlink()
    {
        while (player.FreezeInput)
        {
            await Task.Delay(100);
            spriteRenderer.color = spriteRenderer.color == originalColor ? Color.white : originalColor;
        }
        spriteRenderer.color = originalColor;
    }
}
