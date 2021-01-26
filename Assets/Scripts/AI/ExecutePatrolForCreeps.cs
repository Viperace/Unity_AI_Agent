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

    /* Distance decide weight
     * Level of the lair decide conditions. Lair that is too high level, which is off hero's capability will not be selected
     */
    public void ComputeWeights(List<Lair> candidates)
    {
        // hero level cap
        int heroLevel = actor.GetComponent<RolePlayingStatBehavior>().rpgStat.level;
        float heroAttack = actor.GetComponent<HeroCombatStat>().AttackPower();

        // Convert var to Town object
        candidateWeightsDict = new Dictionary<Lair, float>();
        _totalWeights = 0;
        foreach (Object candidate in candidates)
        {
            if (candidate is Lair t)
            {
                float discount;
                float expectedProbWin = heroAttack / (t.AverageCreepsAttack + heroAttack);
                if (expectedProbWin < 0.15f) 
                    discount = 0f;
                else if(expectedProbWin < 0.3f) 
                    discount = 0.2f;
                else if (expectedProbWin < 0.5f)
                    discount = 0.5f;
                else
                    discount = 1f;

                // Find distance between actor and this 
                float distance = Vector3.Distance(actor.transform.position, t.transform.position);
                float weight = 1 / Mathf.Sqrt(distance); // Formula is 1/sqrt(d) . 1/d too high

                // Final weight
                float finalWeight = weight * discount;

                // Convert to weight
                candidateWeightsDict.Add(t, finalWeight);

                // Save total weight
                _totalWeights += finalWeight;
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
