using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] float ghostDuration = 3f;
    [SerializeField] GameObject PushArea = null;

    [SerializeField] float blinkDuration = 0.1f;

    [SerializeField] float originalSpeed = 0f;
    [SerializeField] float ghostSpeed = 0f;

    List<GameObject> unWalkables = new List<GameObject>();

    MovementController movementController;
    CircleCollider2D myCollider;
    InGameLoopController InGameLoopController;
    Player player;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        myCollider = GetComponent<CircleCollider2D>();
        InGameLoopController = FindObjectOfType<InGameLoopController>();
        player = GetComponent<Player>();
    }

    void Start()
    {
        originalSpeed = InGameLoopController.HunterSpeed;
        ghostSpeed = originalSpeed / 3;
    }

    public void StartGhosting()
    {
        StartCoroutine(HandleGhosting());
    }

    IEnumerator HandleGhosting()
    {
        player.FreezeInput = false;
        spriteRenderer.sharedMaterial.color = player.ghostColor;
        TurnGhostOn(true, ghostSpeed);
        yield return new WaitForSeconds(ghostDuration / 3 * 2);
        StartCoroutine(BlinkOn(ghostDuration / 3));
    }

    private void TurnGhostOn(bool value, float speed)
    {
        player.Dead = value;
        movementController.MovementSpeed = speed;
        PushArea.SetActive(!value);
        myCollider.isTrigger = value;
        if (!value)
            spriteRenderer.sharedMaterial.color = player.originalColor;
    }

    IEnumerator BlinkOn(float time)
    {
        spriteRenderer.sharedMaterial.color = player.originalColor;
        yield return new WaitForSeconds(blinkDuration);
        float newTime = time - blinkDuration;
        if (newTime < 0)
            TryToRevive();
        if(player.Dead)
        StartCoroutine(BlinkOff(newTime));
    }

    IEnumerator BlinkOff(float time)
    {
        spriteRenderer.sharedMaterial.color = player.ghostColor;
        yield return new WaitForSeconds(blinkDuration);
        float newTime = time - blinkDuration;
        if (newTime < 0)
            TryToRevive();
        if(player.Dead)
        StartCoroutine(BlinkOn(newTime));
    }

    private void TryToRevive()
    {
        unWalkables.RemoveAll(gameObject => gameObject == null);
        if (unWalkables.Count > 0)
            return;
        else
            TurnGhostOn(false, originalSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Remove(collision.gameObject);
    }

    public void ResetRespawn()
    {
        StopAllCoroutines();
        movementController.MovementSpeed = originalSpeed;
        PushArea.SetActive(true);
        myCollider.isTrigger = false;
    }
}
