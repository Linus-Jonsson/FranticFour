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
}
