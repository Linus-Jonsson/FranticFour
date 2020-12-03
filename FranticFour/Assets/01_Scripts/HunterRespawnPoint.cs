using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class HunterRespawnPoint : MonoBehaviour
{
    HunterRespawnHandler hunterRespawnHandler;
    void Start()
    {
        hunterRespawnHandler = GetComponentInParent<HunterRespawnHandler>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("entered camera");
        hunterRespawnHandler.AddToActiveSpawnPoint(transform);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print("exit camera");
        hunterRespawnHandler.RemoveActiveSpawnPoint(transform);
    }
}
