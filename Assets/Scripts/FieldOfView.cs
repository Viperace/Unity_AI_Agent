using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    HashSet<Actor> actorsWithinView { get; set; }

    List<Actor> _creepsWithinView; 

    Actor ownActor;

    void Start()
    {
        actorsWithinView = new HashSet<Actor>();        
        ownActor = GetComponentInParent<Actor>();

        _creepsWithinView = new List<Actor>();
    }

    void Update()
    {
           

    }

    void FlushNullItems()
    {
        // Flush destoryed creeps
        _creepsWithinView.RemoveAll(item => item == null);

        // Flush destoryed actors
        actorsWithinView.Remove(null);
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

    public List<Actor> GetCreepsWithinView()
    {
        _creepsWithinView = new List<Actor>();
        foreach(Actor a in actorsWithinView)
            if (a.tag == "Creeps")
                _creepsWithinView.Add(a);

        return _creepsWithinView;
    }

    public HashSet<Actor> GetActorsWithinView()
    {
        // Flush first
        FlushNullItems();
        
        return actorsWithinView;
    }
}
