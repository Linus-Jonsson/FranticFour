using UnityEngine;

public class AssignedController : MonoBehaviour
{
    [Header("Player ID")]
    [Range(0,4)]
    [SerializeField] private int playerID;
    
    [Header("Assigned controlls")]
    [SerializeField] private string vertical;
    [SerializeField] private string horizontal;
    [SerializeField] private string push;

    private void Start()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;
        push = StringManager.Inputs.push + playerID;
    }
}
