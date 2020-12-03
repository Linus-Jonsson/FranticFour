using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraTriggerArea : MonoBehaviour
{
    [SerializeField] float xScale = 0f;
    [SerializeField] float yScale = 0f;

    BoxCollider2D boxCollider2D;
    Camera mainCamera;
    float orthoSize = 0f;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        mainCamera = GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        if(orthoSize != mainCamera.orthographicSize)
        {
            orthoSize = mainCamera.orthographicSize;
            boxCollider2D.size = new Vector2(orthoSize * xScale, orthoSize * yScale);
        }
    }
}
