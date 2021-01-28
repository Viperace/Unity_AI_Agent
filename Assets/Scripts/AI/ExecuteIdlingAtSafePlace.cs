using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteIdlingAtSafePlace : IPlanExecutor
{
    Actor actor;
    public event System.Action OnPlanCompleted;

    public ExecuteIdlingAtSafePlace(Actor actor)
    {
        this.actor = actor;
    }
    public void Execute()
    {
        Vector3 position = SelectCandidateRandomly();
        float duration = DecideStayDuration();
        actor.WanderAround(position, 6, duration);
    }

    float DecideStayDuration()
    {
        return Random.Range(8f, 16f);
    }

    public Vector3 IdentifyCandidates()
    {
        // If no lair, just quit
        if (Lair.lairs == null)
            return actor.transform.position;

        // Find Position that is not near lair
        List<Vector3> nearbyLairsPositions = new List<Vector3>();
        foreach(Lair lair in Lair.lairs)
            if(Vector3.Distance(actor.transform.position, lair.transform.position) < 8f)
                nearbyLairsPositions.Add(lair.transform.position);

        // If no lair nearby, then done
        if (nearbyLairsPositions.Count == 0)
            return actor.transform.position;

        // Find the centroid 
        Vector3 centroid = Vector3.zero;
        foreach (Vector3 pos in nearbyLairsPositions)
            centroid += pos/nearbyLairsPositions.Count;

        // Find an opposite dir
        Vector3 dir = actor.transform.position - centroid;
        Vector3 finalPos = dir.normalized * 10f; // 10 unit from centroid
        return finalPos;
    }

    public Vector3 SelectCandidateRandomly()
    {
        return IdentifyCandidates();
    }
    public void SetOnPlanComplete(System.Action action)
    {
        OnPlanCompleted += action;
    }

    public void Stop() => actor.StopAllActions();

    public void Update()
    {
        // notneeded
    }
}
