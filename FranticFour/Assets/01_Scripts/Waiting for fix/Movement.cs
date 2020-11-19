using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public Controller controller;

    private void Start()
    {
        Debug.Log(controller.horizontal);//Debug
    }

    private void Update()
    {
        float moveSpeed = 10;
        
        //Debug
        float horizontalInput = Input.GetAxis(controller.horizontal);
        float verticalInput = Input.GetAxis(controller.vertical);

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (moveSpeed * Time.deltaTime));

        if (Input.GetAxis(controller.push) > 0f)
        {
            Debug.Log("Pushing!");
        }
    }
}
