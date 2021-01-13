using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public HashSet<Actor> actorsWithinView { get; private set; }

    Actor ownActor;

    void Start()
    {
        actorsWithinView = new HashSet<Actor>();

        ownActor = GetComponentInParent<Actor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor a = other.GetComponentInParent<Actor>();
        if (a && a != ownActor)
            actorsWithinView.Add(a);

        //Debug.Log(ownActor.gameObject.name + " sees enemy: " + actorsWithinView.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        Actor a = other.GetComponentInParent<Actor>();
        if (a && a != ownActor)
            actorsWithinView.Remove(a);

        //Debug.Log(ownActor.gameObject.name + " lost enemy. Total now is " + actorsWithinView.Count);
    }

}
