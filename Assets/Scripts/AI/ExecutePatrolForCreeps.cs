using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutePatrolForCreeps : IPlanExecutor
{
    Actor actor;

    Dictionary<Lair, float> candidateWeightsDict;

    float _totalWeights;

    public event System.Action OnPlanCompleted;

    public ExecutePatrolForCreeps(Actor actor) => this.actor = actor;
    public void Execute()
    {
        Lair lair = (Lair)SelectCandidateRandomly();
        Vector3[] waypoints = GetRandomWaypoints(lair.transform.position).ToArray();

        // Define callback when complete        
        actor.PatrolWaypointsLoop(7f, 4, OnPlanCompleted, waypoints);
    }

    // Ranomly define N waypoints around the epicenter
    List<Vector3> GetRandomWaypoints(Vector3 epicenter)
    {
        List<Vector3> wp = new List<Vector3>();

        // Roll number of wp 
        int N = Random.Range(5, 8);
        Vector3 lastWaypoint = epicenter;
        for (int i = 0; i < N; i++)
        {
            float epsx = Random.value > 0.5f ? 1f : -1f;
            float epsz = Random.value > 0.5f ? 1f : -1f;
            float x = Random.Range(4f, 7f);
            float z = Random.Range(4f, 7f);
            Vector3 pos = lastWaypoint + new Vector3(x * epsx, epicenter.y, z * epsz);

            // Update 
            lastWaypoint = (lastWaypoint + epicenter) * 0.5f;
            //Save 
            wp.Add(pos);
        }

        return wp;
    }

    public Object SelectCandidateRandomly()
    {
        List<Lair> lairs = IdentifyCandidates<Lair>();

        float[] probs = new float[lairs.Count];
        for (int i = 0; i < lairs.Count; i++)
        {
            probs[i] = GetProbability(lairs[i]);
        }

        int roll = MyStatistics.RandomWeightedIndex(probs);
        return lairs[roll];
    }

    public void ComputeWeights(List<Lair> candidates)
    {
        // Convert var to Town object
        candidateWeightsDict = new Dictionary<Lair, float>();
        _totalWeights = 0;
        foreach (Object candidate in candidates)
        {
            if (candidate is Lair)
            {
                Lair t = (Lair)candidate;

                // Find distance between actor and this 
                float distance = Vector3.Distance(actor.transform.position, t.transform.position);
                float weight = 1 / Mathf.Sqrt(distance); // Formula is 1/sqrt(d) . 1/d too high

                // Convert to weight
                candidateWeightsDict.Add(t, weight);

                // Save total weight
                _totalWeights += weight;
            }
        }
    }

    public List<Lair> IdentifyCandidates<T>()
    {
        List<Lair> candidates = new List<Lair>(Lair.lairs);
        ComputeWeights(candidates);
        return candidates;
    }

    float GetProbability(object candidate)
    {
        if (candidate is Lair)
            return candidateWeightsDict[(Lair)candidate] / _totalWeights;
        else
            return 0;
    }

    public void Stop()
    {
        actor.StopAllActions();
    }

    public void Update()
    {
    }

    public void SetOnPlanComplete(System.Action action)
    {        
        OnPlanCompleted += action;
    }
}
