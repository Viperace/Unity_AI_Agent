using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewedByCamera : MonoBehaviour
{
    public static HashSet<ViewedByCamera> viewedActors;

    public bool IsVisible { get; private set; }
    private void Awake()
    {
        // Register		
        if (viewedActors == null)
            viewedActors = new HashSet<ViewedByCamera>();
        viewedActors.Add(this);
        viewedActors.Remove(null);
    }
    void OnDestroy()
    {
        if (viewedActors != null)
            viewedActors.Remove(this);
    }
    private void OnBecameVisible()
    {
        IsVisible = true;
    }

    private void OnBecameInvisible()
    {
        IsVisible = false;
    }
}
