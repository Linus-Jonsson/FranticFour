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

        //Controller assigned (debug)
        //Debug.Log(controllerNum);
        //Debug.Log(string.Format(
        //    "Controller: {0} Vertical: {1} \nHorizontal: {2} \nPush: {3}",
        //    m_name, horizontal, vertical, push));
    }

    private void AssignInputs()
    {
        Debug.LogWarning(m_id);
        vertical = StringManager.Inputs.vertical + m_id;
        horizontal = StringManager.Inputs.horizontal + m_id;
        push = StringManager.Inputs.push + m_id;
    }
}