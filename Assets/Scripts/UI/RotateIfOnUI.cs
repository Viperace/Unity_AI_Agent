using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIfOnUI : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 50f;

    RectTransform parentRect;
    void Start()
    {
        if(transform.parent && GetComponentInParent<RectTransform>())
        {
            parentRect = GetComponentInParent<RectTransform>();
            rotateSpeed += Random.Range(-10, 10);
        }
    }

    void Update()
    {
        if (parentRect)
        {            
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
