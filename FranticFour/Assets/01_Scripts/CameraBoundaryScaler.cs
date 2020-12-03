using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBoundaryScaler : MonoBehaviour
{
    [SerializeField] float scaler = 1f;
    [SerializeField] float offset = 0.5f;

    [SerializeField] bool positive = false;
    [SerializeField] bool vertical = false;


    BoxCollider2D boxCollider;
    Camera mainCamera;

    float orthoSize = 0f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = GetComponentInParent<Camera>();
    }

    private void FixedUpdate()
    {
        if (orthoSize != mainCamera.orthographicSize)
            SetBoundarySizeAndPosition();

    }

    private void SetBoundarySizeAndPosition()
    {
        orthoSize = mainCamera.orthographicSize;
        if (vertical)
        {
            boxCollider.size = new Vector2(1, scaler * orthoSize);
            float trueOffset = offset * orthoSize;
            if (positive)
                boxCollider.offset = new Vector2(trueOffset, 0);
            else
                boxCollider.offset = new Vector2(-trueOffset, 0);
        }
        else
        {
            boxCollider.size = new Vector2(scaler * orthoSize, 1);
            float trueOffset = offset + orthoSize;
            if (positive)
                boxCollider.offset = new Vector2(0, trueOffset);
            else
                boxCollider.offset = new Vector2(0, -trueOffset);
        }
    }
}
