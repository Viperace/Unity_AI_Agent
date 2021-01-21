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
		//return Random.Range(15f, 30f);
		return 10f;
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

	public void Stop() => actor.StopAllActions();

	public void SetOnPlanComplete(System.Action action)
	{
		// Don't need to set here. Once enter town, gameObject will be inactive anyway. So this is useless
		OnPlanCompleted += action;
	}
}


public class ExecuteGoShopping : IPlanExecutor
{
	Actor actor;
	Dictionary<Merchant, float> candidateWeightsDict;
	float _totalWeights;

	public event System.Action OnPlanCompleted;

	public ExecuteGoShopping(Actor actor)
	{
		this.actor = actor;
	}

	public List<Merchant> IdentifyCandidates<T>()
	{
		List<Merchant> candidates = new List<Merchant>(Merchant.shops);
		ComputeWeights(candidates);
		return candidates;
	}

	public void ComputeWeights(List<Merchant> candidates)
	{
		// Convert var to Town object
		candidateWeightsDict = new Dictionary<Merchant, float>();
		_totalWeights = 0;
		foreach (Object candidate in candidates)
		{
			if (candidate is Town)
			{
				Merchant t = (Merchant)candidate;

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
			return candidateWeightsDict[(Merchant)candidate] / _totalWeights;
		else
			return 0;
	}

	// Based on probability 
	public Object SelectCandidateRandomly()
	{
		List<Merchant> shops = IdentifyCandidates<Merchant>();

		//Dictionary<Town, float> townProbability = new Dictionary<Town, float>();
		float[] probs = new float[shops.Count];
		for (int i = 0; i < shops.Count; i++)
		{
			probs[i] = GetProbability(shops[i]);
		}

		int roll = MyStatistics.RandomWeightedIndex(probs);
		return shops[roll];
	}

	float DecideStayDuration()
	{
		//return Random.Range(15f, 30f);
		return 2f;
	}

	// Call from update cycle
	public void Execute()
	{
		var shop = SelectCandidateRandomly();
		float duration = DecideStayDuration();

		actor.GotoShopAndShopping((Merchant)shop, duration);
	}
	public void Update()
	{
		// notneeded
	}

	public void Stop() => actor.StopAllActions();

	public void SetOnPlanComplete(System.Action action)
	{
		// Don't need to set here. Once enter town, gameObject will be inactive anyway. So this is useless
		OnPlanCompleted += action;
	}
}

