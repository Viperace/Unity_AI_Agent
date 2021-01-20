using System.Collections.Generic;
using UnityEngine;

public class ExecuteGoTown : IPlanExecutor
{
	Actor actor;
	Dictionary<Town, float> candidateWeightsDict;
	float _totalWeights;

	public event System.Action OnPlanCompleted;

	public ExecuteGoTown(Actor actor)
	{
		this.actor = actor;
	}

	public List<Town> IdentifyCandidates<T>()
	{
		List<Town> candidates = new List<Town>(Town.towns);
		ComputeWeights(candidates);
		return candidates;
	}

	public void ComputeWeights(List<Town> candidates)
	{
		// Convert var to Town object
		candidateWeightsDict = new Dictionary<Town, float>();
		_totalWeights = 0;
		foreach (Object candidate in candidates)
		{
			if (candidate is Town)
			{
				Town t = (Town)candidate;

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

	public float GetProbability(object candidate)
	{
		if (candidate is Town)
			return candidateWeightsDict[(Town)candidate] / _totalWeights;
		else
			return 0;
	}

	// Based on probability 
	public Object SelectCandidateRandomly()
	{
		List<Town> towns = IdentifyCandidates<Town>();

		//Dictionary<Town, float> townProbability = new Dictionary<Town, float>();
		float[] probs = new float[towns.Count];
		for (int i = 0; i < towns.Count; i++)
		{
			probs[i] = GetProbability(towns[i]);
		}

		int roll = MyStatistics.RandomWeightedIndex(probs);
		return towns[roll];
	}

	float DecideStayDuration()
	{
		return Random.Range(15f, 30f);
	}

	// Call from update cycle
	public void Execute()
	{
		var town = SelectCandidateRandomly();
		float duration = DecideStayDuration();
		actor.GotoTownAndEnter((Town)town, duration);
	}
	public void Update()
	{
		// notneeded
	}

	public void Stop()
	{
		// Actor can set sequeceion and indivcidual action
		//Actor can stop all actions and sequence
		actor.SetCurrentAction(null);
		actor.SetCurrentActionSequence(null);
	}

	public void SetOnPlanComplete(System.Action action)
	{
		OnPlanCompleted += action;
	}
}

