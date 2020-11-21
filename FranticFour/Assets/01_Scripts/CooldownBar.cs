using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownBar : MonoBehaviour
{
    [Header("Set in editor")] [SerializeField]
    private Transform player;

    [SerializeField] private Color fullBarColor;
    [SerializeField] private Color emptyBarColor;
    private GameObject parentGObj;

    //Cooldown bar
    [Header("Debug")] private float timeLeft;
    private float coolDownTime;
    [SerializeField] private bool barActive;
    [SerializeField] private Vector3 emptyBarPos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 fullBarPos = new Vector3(0, 0, 0);
    [SerializeField] private float yOffset = -0.65f;
    private SpriteRenderer render;
    //Full
    //Full green

    private void Start()
    {
        parentGObj = transform.parent.gameObject;
        render = GetComponent<SpriteRenderer>();

        //Player
        PlayerActionsController playerActionController = player.GetComponent<PlayerActionsController>();
        coolDownTime = playerActionController.PushCooldown;
        playerActionController.OnPush.AddListener(StartBar);

        emptyBarPos = new Vector3(-GetComponent<Renderer>().bounds.size.x, 0, 0);
    }

    private void Update()
    {
        //Updates cooldown bar pos to match player
        Vector3 playerPos = player.position;
        parentGObj.transform.position = new Vector3(
            playerPos.x,
            playerPos.y + yOffset,
            playerPos.z);

        if (barActive)
            BarActive();
    }

    private void StartBar()
    {
        barActive = true;
        timeLeft = coolDownTime;
        render.color = emptyBarColor;
        transform.localPosition = emptyBarPos;
    }

    private void BarActive()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            transform.localPosition = new Vector3(((emptyBarPos.x / coolDownTime) * timeLeft), 0, 0);
            return;
        }
        transform.localPosition = fullBarPos;
        render.color = fullBarColor;
        barActive = false;
    }
}