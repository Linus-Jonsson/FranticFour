using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [Tooltip("The speed will be a number between X (min) and Y (max inklusive)")]
    [SerializeField] Vector2 speedRange = new Vector2(0,0);
    [Tooltip("The X-scale will be a number between X (min) and Y (max inklusive)")]
    [SerializeField] Vector2 xScaleRange = new Vector2(0, 0);
    [Tooltip("The Y-scale will be a number between X (min) and Y (max inklusive)")]
    [SerializeField] Vector2 yScaleRange = new Vector2(0, 0);
    [Tooltip("The Y-spawn position will be a number between X (min) and Y (max inklusive)")]
    [SerializeField] Vector2 ySpawnRange = new Vector2(0, 0);
    [SerializeField] float xStartPoint = 0;
    [SerializeField] float xEndPoint = 0;

    Vector3 targetPosition = new Vector3(0,0,0);
    float speed = 0f;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        float yPosition = Random.Range(ySpawnRange.x, ySpawnRange.y);
        float scale = yPosition / 4;
        transform.localScale = new Vector3(scale, scale, 0);
        speed = yPosition / 5;
        transform.position = new Vector3(transform.position.x, yPosition, 0);
        targetPosition = new Vector3(xEndPoint, yPosition, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCloud();
    }

    private void MoveCloud()
    {
        float movementThisUpdate = speed * Time.fixedDeltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisUpdate);
        if (transform.position == targetPosition)
        {
            ResetCloud();
        }
    }

    private void ResetCloud()
    {
        float yPosition = Random.Range(ySpawnRange.x, ySpawnRange.y);
        float scale = yPosition / 5;
        transform.localScale = new Vector3(scale,scale,0);
        speed = yPosition / 5;
        
        spriteRenderer.sortingOrder = Mathf.RoundToInt(yPosition);

        transform.position = new Vector3(xStartPoint, yPosition, yPosition);
        targetPosition = new Vector3(xEndPoint, yPosition, 0);
    }
}
