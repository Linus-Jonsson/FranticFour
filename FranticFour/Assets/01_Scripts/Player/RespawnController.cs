using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] float ghostDuration = 3f;
    [SerializeField] GameObject PushArea = null;

    [SerializeField] float blinkDuration = 0.1f;

    [SerializeField] List<GameObject> unWalkables = new List<GameObject>();
    [SerializeField] CircleCollider2D bodyCollider = null;

    MovementController movementController;
    InGameLoopController InGameLoopController;
    Player player;

    float hunterSpeed = 200f;
    float preySpeed = 164f;
    float ghostSpeed = 0f;
    float originalColliderRadius = 0;
    float ghostColliderRadius = 0;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        InGameLoopController = FindObjectOfType<InGameLoopController>();
        player = GetComponent<Player>();
    }



    void Start()
    {
        originalColliderRadius = bodyCollider.radius;
        ghostColliderRadius = originalColliderRadius * 3;
        hunterSpeed = InGameLoopController.HunterSpeed;
        preySpeed = InGameLoopController.PreySpeed;
        
        hunterSpeed = 200f;
        preySpeed = 164f;//Nicklas får fixa nån gång
        ghostSpeed = hunterSpeed / 3;
    }

    public void StartGhosting()
    {
        if (player.Prey || InGameLoopController.CurrentPrey.Dead) { return; }
        player.SetAnimationBool("StayDead", false);
        bodyCollider.radius = ghostColliderRadius;
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
        bodyCollider.isTrigger = value;
        if (!value)
        {
            movementController.ResetMovement();
            spriteRenderer.sharedMaterial.color = player.originalColor;
            bodyCollider.radius = originalColliderRadius;
        }
    }

    IEnumerator BlinkOn(float time)
    {
        spriteRenderer.sharedMaterial.color = player.originalColor;
        yield return new WaitForSeconds(blinkDuration);
        float newTime = time - blinkDuration;
        if (newTime < 0)
            TryToRevive();
        if (player.Dead)
            StartCoroutine(BlinkOff(newTime));
    }

    IEnumerator BlinkOff(float time)
    {
        spriteRenderer.sharedMaterial.color = player.ghostColor;
        yield return new WaitForSeconds(blinkDuration);
        float newTime = time - blinkDuration;
        if (newTime < 0)
            TryToRevive();
        if (player.Dead)
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
        if (collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Add(collision.gameObject);
        if (unWalkables.Count > 0 && player.Dead)
            bodyCollider.radius = ghostColliderRadius;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Unwalkable"))
            unWalkables.Remove(collision.gameObject);
        if (unWalkables.Count <= 0)
            bodyCollider.radius = originalColliderRadius;
    }

    public void ResetRespawn()
    {
        StopAllCoroutines();
        unWalkables = new List<GameObject>();
        movementController.MovementSpeed = player.Prey ? preySpeed : hunterSpeed;
        PushArea.SetActive(true);
        bodyCollider.isTrigger = false;
    }
}
