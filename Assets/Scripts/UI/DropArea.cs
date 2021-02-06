using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DropArea : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public bool ReceiveTarget(DragTransform dragTransform)
    {
        if (CheckIfMouseWithinRect() & !IsOccupied())
        {
            dragTransform.transform.SetParent(this.transform);
            dragTransform.transform.localPosition = new Vector3(0, 0, -100f);
            return true;
        }
        else
            return false;
    }

    public bool CheckIfMouseWithinRect()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main);
    }

    public bool IsOccupied()
    {
        if (GetComponentInChildren<DragTransform>() == null)
            return false;
        return true;
    }
}
