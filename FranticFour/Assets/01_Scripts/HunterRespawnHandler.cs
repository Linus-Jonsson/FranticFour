using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class HunterRespawnHandler : MonoBehaviour
{
    [SerializeField] Transform[] activeSpawnPoints = null;
    [SerializeField] float minDistance = 10f;
    [SerializeField] float maxDistance = 20f;

    [SerializeField] Transform prey = null; // remove serializeField once all works
    [SerializeField] private bool trySpawn = false;

    private void Start()
    {
        activeSpawnPoints = GetComponentsInChildren<Transform>();
    }

    private void SHitFuckFuck()
    {
        if (trySpawn) GetSpawnPoint();
    }

    public Transform GetSpawnPoint()
    {
        trySpawn = true;
        if (prey is null)
            SHitFuckFuck();
        trySpawn = false;

        foreach (var transform in activeSpawnPoints)
        {
            float xDistance = Mathf.Abs(transform.position.x - prey.position.x);
            float yDistance = Mathf.Abs(transform.position.y - prey.position.y);
            if (xDistance > minDistance && xDistance < maxDistance && yDistance > minDistance &&
                yDistance < maxDistance)
                return transform;
        }

        return activeSpawnPoints[0];
    }

    public void SetPrey(Transform prey)
    {
        this.prey = prey;
    }
}