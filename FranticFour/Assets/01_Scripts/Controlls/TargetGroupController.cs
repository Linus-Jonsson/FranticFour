using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// can we remove this stuff? 

public class TargetGroupController : MonoBehaviour
{
    [SerializeField] int preyWeight = 12;
    [SerializeField] int hunterWeight = 6;
    [SerializeField] float preyRadius = 1.5f;
    [SerializeField] float hunterRadius = 1f;

    CinemachineTargetGroup cinemachineGroup;

    private void Start()
    {
        cinemachineGroup = FindObjectOfType<CinemachineTargetGroup>();
    }

    public void UpdateTargetGroup(Player[] players)
    {
        foreach (var player in players)
        {
            cinemachineGroup.RemoveMember(player.transform);

            if (player.Prey)
            {
                cinemachineGroup.AddMember(player.transform, preyWeight, preyRadius);
            }
            else
            {
                cinemachineGroup.AddMember(player.transform, hunterWeight, hunterRadius);
            }
        }
    }

}
