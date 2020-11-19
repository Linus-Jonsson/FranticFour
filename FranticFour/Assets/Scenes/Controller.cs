using UnityEngine;

[System.Serializable]
public class Controller
{
    private string m_name;
    private int m_id;
    private int m_controllerNum;
    
    public string Name => m_name;

    public string vertical;
    public string horizontal;
    public string push;

    public Controller(string name, int id, int controllerNum)
    {
        m_name = name;
        m_id = id;
        m_controllerNum = controllerNum;

        AssignInputs();
        LinkInputManager.LinkController(controllerNum, m_id);
    }

    private void AssignInputs()
    {
        Debug.LogWarning(m_id);
        vertical = StringManager.Inputs.vertical + m_id;
        horizontal = StringManager.Inputs.horizontal + m_id;
        push = StringManager.Inputs.push + m_id;
    }
}