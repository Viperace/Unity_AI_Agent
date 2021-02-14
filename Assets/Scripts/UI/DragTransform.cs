using System.Collections;
using UnityEngine;
using System;

public class DragTransform : MonoBehaviour
{
    private Color mouseOverColor = Color.blue;
    private Color originalColor = Color.white;
    private bool dragging = false;
    private float distance;
    private Vector3 origLocalPos;

    Renderer myRenderer;
    DropArea[] dropAreas;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        dropAreas = FindObjectsOfType<DropArea>(true);
    }


    void OnMouseEnter()
    {
        myRenderer.material.color = mouseOverColor;
    }

    void OnMouseExit()
    {
        myRenderer.material.color = originalColor;
    }

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        origLocalPos = transform.parent.localPosition;
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;

        // ANy drop area receive this ?
        foreach(DropArea a in dropAreas)
            if (a.ReceiveTarget(this))
                return;
        
        // If no, snap back.  This assuming COllider is one level child of the parent
        transform.parent.localPosition = origLocalPos;
    }


    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance - 10f); // If zero, what clip behind camera
            //transform.position = rayPoint;
            transform.parent.position = rayPoint;
        }
    }
}
