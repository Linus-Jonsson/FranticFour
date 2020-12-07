using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownBar : MonoBehaviour
{
    [Header("Set in editor")] 
    [SerializeField] private Transform player = null;

    [Header("Hunter")]
    [SerializeField] private Color fullBarColor = new Color(0,0,0);
    [SerializeField] private Color emptyBarColor = new Color(0, 0, 0);
    [Header("Pray")]
    [SerializeField] private Color fullBarColorPray = new Color(0,0,0);
    [SerializeField] private Color emptyBarColorPray = new Color(0, 0, 0);
    [Header("Player")]
    [SerializeField] private SpriteRenderer backgroundBar = null;
    [SerializeField] private Color actualEmptyBarColor = new Color(0, 0, 0);
    
    private Color activeFullBarColor = new Color();
    private GameObject parentGObj;

    //Cooldown bar
    [Header("Debug")] private float timeLeft;
    private float coolDownTime;
    private float trapsCoolDown;
    private float activeCoolDownTime;
    [SerializeField] private bool isPray; //I will pray for you! Amen
    [SerializeField] private bool barActive;
    [SerializeField] private Vector3 emptyBarPos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 fullBarPos = new Vector3(0, 0, 0);
    [SerializeField] private float yOffset = -0.65f;
    private PlayerActionsController playerActionController;
    private SpriteRenderer render;
    //Full
    //Full green

    private void Start()
    {
        parentGObj = transform.parent.gameObject;
        render = GetComponent<SpriteRenderer>();

        //Player
        playerActionController = player.GetComponent<PlayerActionsController>();
        
        isPray = playerActionController.Player.Prey;
        if (isPray)
            render.color = fullBarColorPray;

        coolDownTime = playerActionController.PushCooldown;
        trapsCoolDown = playerActionController.TrapsCoolDown;
        
        playerActionController.Player.BecamePray.AddListener(BecamePray);
        playerActionController.OnPush.AddListener(StartBar);
        playerActionController.OnTrapThrow.AddListener(StartBarPrey);

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

    private void BecamePray()
    {
        if (playerActionController.Player.Prey)
        {
            render.color = fullBarColorPray;
            backgroundBar.color = emptyBarColorPray;
            isPray = true;
        }
        else
        {
            render.color = fullBarColor;
            backgroundBar.color = emptyBarColor;
            isPray = false;
        }
    }

    private void StartBarPrey()
    { //Prey
        barActive = true;
        timeLeft = trapsCoolDown;
        activeCoolDownTime = trapsCoolDown;
        render.color = actualEmptyBarColor;
        transform.localPosition = emptyBarPos;
        activeFullBarColor = fullBarColorPray;
    }

    private void StartBar()
    { //Hunter
        barActive = true;
        timeLeft = coolDownTime;
        activeCoolDownTime = coolDownTime;
        render.color = actualEmptyBarColor;
        transform.localPosition = emptyBarPos;
        activeFullBarColor = fullBarColor;
    }

    private void BarActive()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            transform.localPosition = new Vector3(((emptyBarPos.x / activeCoolDownTime) * timeLeft), 0, 0);
            return;
        }
        transform.localPosition = fullBarPos;
        render.color = activeFullBarColor;
        barActive = false;
    }
}