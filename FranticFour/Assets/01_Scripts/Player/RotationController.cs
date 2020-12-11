using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] GameObject objectToRotate = null;
    AssignedController controller;

    Vector2 dir = new Vector2(0, 0);
    public Vector2 Dir { get { return dir; } }

    void Start()
    {
        controller = GetComponent<AssignedController>();
    }

    void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        transform.rotation = Quaternion.identity;
        if (controller.UsesMouse)
            HandleMouseRotation(); //Musen skriver över kontroller inputs
        else
            HandleControllerRotation();
    }

    private void HandleControllerRotation()
    {
        float inputX = Input.GetAxis(controller.RightHorizontal);
        float inputY = Input.GetAxis(controller.RightVertical);
        if (inputX != 0)
            dir.x = inputX;
        if (inputY != 0)
            dir.y = inputY;
        objectToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }

    private void HandleMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
        objectToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
}
