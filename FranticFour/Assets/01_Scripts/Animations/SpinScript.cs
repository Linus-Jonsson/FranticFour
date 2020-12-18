using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;

    Vector3 rotation = new Vector3(0,0,0);

    void Update()
    {
        rotation += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(rotation);
    }
}
