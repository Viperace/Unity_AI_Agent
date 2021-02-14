using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIfOnUI : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 50f;

    RectTransform parentRect;
    Vector3 pivotOffset;
    void Start()
    {
        if(transform.parent && GetComponentInParent<RectTransform>())
        {
            parentRect = GetComponentInParent<RectTransform>();
            rotateSpeed += Random.Range(-10, 10);

            // To tilt ?
            //this.transform.Rotate(Vector3.forward * Random.Range(8, 10));
        }

    }

    void Update()
    {
        if (parentRect)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            //transform.RotateAround(FindFinalPivot(), Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

    Vector3 FindFinalPivot()
    {
        Renderer m_Collider = GetComponentInChildren<Renderer>();
        if (m_Collider)
        {
            Vector3 tets = 0.5f * (m_Collider.bounds.max + m_Collider.bounds.min);
            return tets;
        }
        else
            return this.transform.position;
    }

}
