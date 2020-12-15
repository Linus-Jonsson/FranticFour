using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] GameObject objectToRotate = null;
    AssignedController controller;

    Vector2 dir = new Vector2(0, 0);

    public Vector2 Dir
    {
        get { return dir; }
    }

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

        if (inputLeft.magnitude != 0)
        {
            //Debug.Log(inputRight.magnitude + "|" + inputLeft.magnitude);
        }
        
        if (inputRight.magnitude > DeadZones.DEADZONE_RIGHT)
            objectToRotate.transform.rotation =
                Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-inputRight.x, inputRight.y) * Mathf.Rad2Deg));
        else if (inputLeft.magnitude > DeadZones.DEADZONE_LEFT)
            objectToRotate.transform.rotation =
                Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-inputLeft.x, inputLeft.y) * Mathf.Rad2Deg));
    }

    private void HandleMouseRotation()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - position;
        objectToRotate.transform.rotation =
            Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg));
    }
}