using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] float ghostDuration = 3f;
    [SerializeField] GameObject PushArea = null;

    [SerializeField] float blinkDuration = 0.1f;

    [SerializeField] float hunterSpeed = 0f;
    [SerializeField] float ghostSpeed = 0f;

    [SerializeField] List<GameObject> unWalkables = new List<GameObject>();

    MovementController movementController;
    CircleCollider2D myCollider;
    InGameLoopController InGameLoopController;
    Player player;

    float originalColliderRadius = 0;
    float ghostColliderRadius = 0;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        myCollider = GetComponent<CircleCollider2D>();
        InGameLoopController = FindObjectOfType<InGameLoopController>();
        player = GetComponent<Player>();
    }

    void Start()
    {
        originalColliderRadius = myCollider.radius;
        ghostColliderRadius = originalColliderRadius * 3;
        hunterSpeed = InGameLoopController.HunterSpeed;
        ghostSpeed = hunterSpeed / 3;
    }

    public void StartGhosting()
    {
        if(player.Prey) { return; }
        myCollider.radius = ghostColliderRadius;
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
        {
            spriteRenderer.sharedMaterial.color = player.originalColor;
            myCollider.radius = originalColliderRadius;
        }

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
            TurnGhostOn(false, hunterSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Add(collision.gameObject);
        if(unWalkables.Count > 0 && player.Dead)
            myCollider.radius = ghostColliderRadius;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Remove(collision.gameObject);
        if(unWalkables.Count <= 0)
            myCollider.radius = originalColliderRadius;
    }

    public void ResetRespawn()
    {
        unWalkables = new List<GameObject>();
        StopAllCoroutines();
        movementController.MovementSpeed = hunterSpeed;
        PushArea.SetActive(true);
        myCollider.isTrigger = false;
    }
}
