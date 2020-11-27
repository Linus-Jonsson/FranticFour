using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerFuckedController : MonoBehaviour
{
    [Header("Push configuration")]
    [Tooltip("The time in seconds that the player getting pushed wont be able to move")]
    [SerializeField] float pushDuration = 0.3f;

    Rigidbody2D rb2d;
    Player player;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    public void GetPushed(Vector2 pushForce)
    {
        if (!player.FreezeInput)
            StartCoroutine(HandlePush(pushForce));
    }
    IEnumerator HandlePush(Vector2 pushForce)
    {
        player.FreezeInput = true;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(pushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(pushDuration);
        player.FreezeInput = false;
    }

    public void GetStunned(float duration)
    {
        StopCoroutine(HandlePush(new Vector2(0, 0)));
        StartCoroutine(HandleStun(duration));
    }

    IEnumerator HandleStun(float duration)
    {
        player.FreezeInput = true;
        player.StunBlink();
        yield return new WaitForSeconds(duration);
        player.FreezeInput = false;
    }
}
