using UnityEngine;

public class Controller
{
    private string m_name;
    private int m_id;
    private int m_controllerNum;

    public Controller(string name, int id, int controllerNum)
    {
        m_name = name;
        m_id = id;
        m_controllerNum = controllerNum;
        
        //Controller assigned
        Debug.Log("Controller assigned id: " + m_id + "|" + controllerNum);
    }
}