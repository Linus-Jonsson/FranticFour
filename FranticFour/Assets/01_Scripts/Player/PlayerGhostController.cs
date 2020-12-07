using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostController : MonoBehaviour
{
    [SerializeField] float ghostDuration = 3f;
    [SerializeField] GameObject PushArea = null;
    [SerializeField] Color ghostColor = new Color(0, 0, 0, 0);
    [SerializeField] Color originalColor = new Color(0, 0, 0, 0);
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float originalSpeed;
    [SerializeField] float ghostSpeed;

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
        originalColor = spriteRenderer.color;
        originalSpeed = InGameLoopController.HunterSpeed;
        ghostSpeed = originalSpeed / 3;
    }

    private void Update()
    {
        // find a way to controll this in a better way
        if (player.Dead)
            spriteRenderer.color = ghostColor;
        else
            spriteRenderer.color = originalColor;
    }

    public void StartGhosting()
    {
        StartCoroutine(HandleGhosting());
    }

    IEnumerator HandleGhosting()
    {
        player.Dead = true;
        movementController.MovementSpeed = ghostSpeed;
        PushArea.SetActive(false);
        myCollider.enabled = false;
        yield return new WaitForSeconds(ghostDuration / 3 * 2);
        player.Dead = false;
        movementController.MovementSpeed = originalSpeed;
        PushArea.SetActive(true);
        myCollider.enabled = true;
    }

    public void ResetRespawn()
    {
        StopAllCoroutines();
        movementController.MovementSpeed = originalSpeed;
        PushArea.SetActive(true);
        myCollider.enabled = true;
    }
}
