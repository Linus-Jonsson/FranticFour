using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HunterRespawnHandler : MonoBehaviour
{
    [SerializeField] Transform[] activeSpawnPoints = null;
    [SerializeField] float minDistance = 10f;
    [SerializeField] float maxDistance = 20f;

    [SerializeField] Transform prey = null; // remove serializeField once all works
     
    private void Start()
    {
        activeSpawnPoints = GetComponentsInChildren<Transform>();
    }

    public Transform GetSpawnPoint()
    {
        foreach (var transform in activeSpawnPoints)
        {
            float xDistance = Mathf.Abs(transform.position.x - prey.position.x);
            float yDistance = Mathf.Abs(transform.position.y - prey.position.y);
            if(xDistance > minDistance && xDistance < maxDistance && yDistance > minDistance && yDistance < maxDistance)
            {
                return transform;
            }
        }
        return activeSpawnPoints[0];
    }

    public void SetPrey(Transform prey)
    {
        this.prey = prey;
    }
}
