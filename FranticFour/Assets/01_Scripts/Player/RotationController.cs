using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] GameObject objectToRotate = null;
    AssignedController controller;

    Vector2 dir = new Vector2(0, 0);

    public Vector2 Dir => dir;

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
        Vector2 inputRight = new Vector2(Input.GetAxis(controller.RightHorizontal), Input.GetAxis(controller.RightVertical));
        Vector2 inputLeft = new Vector2(Input.GetAxis(controller.Horizontal), Input.GetAxis(controller.Vertical));

        if (inputRight.magnitude > DeadZones.DEADZONE_RIGHT)
        {
            dir = new Vector2(inputRight.x, inputRight.y);
            float rotation = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            objectToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }            
        else if (inputLeft.magnitude > DeadZones.DEADZONE_LEFT)
        {
            dir = new Vector2(inputLeft.x, inputLeft.y);
            float rotation = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            objectToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }
            
    }

    private void HandleMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = (Input.mousePosition - position).normalized;
        objectToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
}