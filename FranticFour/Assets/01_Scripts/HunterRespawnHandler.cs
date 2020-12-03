using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HunterRespawnHandler : MonoBehaviour
{
    [SerializeField] List<Transform> activeSpawnPoints = new List<Transform>();

    public void AddToActiveSpawnPoint(Transform spawnPoint)
    {
        activeSpawnPoints.Add(spawnPoint);
    }
    public void RemoveActiveSpawnPoint(Transform spawnPoint)
    {
        activeSpawnPoints.Remove(spawnPoint);
        activeSpawnPoints.RemoveAll(point => point == null);
    }

    public Transform GetSpawnPoint()
    {
        int random = Random.Range(0, activeSpawnPoints.Count);
        return activeSpawnPoints[random];
    }
}
