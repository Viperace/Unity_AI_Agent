/* How Brain & Plan works 
Brain -> 
- Evaluate every fixed interval, or when
	Plan completed
	
- Loop through every HIGH_LEVEL_PLANS.enum,
	(skipping plan that is not valid i.e no target)
	- get from dictionary the needs+
	- evalu Marginal Utility

- Has an attribute 'PlanExecutor' which always updating
PlanExecutor
- PlanExecutor.Perform the Plan form best Marginal utility
(future can do bad plan with low probability)

	
PlanExecutor is interface class to execute each plan 


Idle = 0,
	wander around 
GoTownAndSleep,
	prob weight = 1/distance
EnactTentAndSleep?,
	random nearby
	(filter for empty area)
GoShopping,
	prob weight = 1/distance
FleeFromDanger?,
	find centroid of all threats.
	Move to opposite direction of centroid
	keep updating centroid 
PatrolAreaForCreep,
	Find all Lairs
	prob weight = equal
	[future: select one that is closest to hero level]
HuntCreep?
*/

using UnityEngine;
using System.Collections.Generic;

public interface IPlanExecutor
{
	public List<T> IdentifyCandidates<T>();	
	//public float GetProbability(Object candidate);	
	public Object SelectCandidateRandomly();
	public void Execute(); 
	public void Stop();
}

// Lairs
public class ExecuteGoTown: IPlanExecutor
{
	Actor actor;
	Dictionary<T, float> candidateWeightsDict;
	float _totalWeights;
	
	
	public ExecuteGoTown(Actor actor)
	{
		this.actor = actor;		
	}
	
	public List<Town> IdentifyCandidates<Town>()
	{
		// Find all towns within game
		List<Town> candidates = new List<Town>();
		candidates.Add(FindAllObjectsOfTypes<Town>());
		
		return candidates;
	}
	
	public void ComputeWeights(List<Object> candidates)
	{
		// Convert var to Town object
		candidateWeightsDict = new Dictionary<Town, float>();	
		_totalWeights = 0;
		foreach(Object candidate in candidates){
			if(candidate is Town){
				Town t = (Town) candidate;
				
				// Find distance between actor and this 
				float distance = Vector3.Distance(actor.transform.position, t.transform.position);
				float weight = 1/Mathf.Sqrt(distance); // Formula is 1/sqrt(d) . 1/d too high
				
				// Convert to weight
				candidateWeightsDict.Add(t, weight);
				
				// Save total weight
				_totalWeights += weight;
			}
		}								
	}
	
	public float GetProbability(Object candidate)
	{
		return candidateWeightsDict[candidate]/_totalWeights;		
	}
	
	// Based on probability 
	public Object SelectCandidateRandomly()
	{
		List<Town> towns = IdentifyCandidates<Town>();
		
		//Dictionary<Town, float> townProbability = new Dictionary<Town, float>();
		float[] probs = new float[towns.Count];
		for(int i =0; i < towns.Count; i++){
			probs[i] = GetProbability(towns[i]);
		}
		
		int roll = MyStatistics.RandomWeightedIndex(probs);
		return towns[roll];
	}


	public void Execute()
	{
		// Initialize the plan 
		//actor.GoTown
	}
	
	public void Stop() 
	{	
	
		// Actor can set sequeceion and indivcidual action
		//Actor can stop all actions and sequence
		actor.SetCurrentAction(null);
	}
}


/* Stochastics and statistcs
*/
public static class MyStatistics
{
	
	public static int RandomWeightedIndex(int[] x)
	{
		// Find cumsum
		int[] xcum = new int[x.Length];
		for(int i = 0; i < x.Length;i++)
			if(i == 0)
				xcum[i] = x[i];
			else
				xcum[i] = xcum[i - 1] + x[i];
		
		
		// Roll
		float rollVal = Random.Range(0, xcum[xcum.Length - 1]);
		
		// Decide which bucket it falls in.
		for(int i = 1; i < x.Length;i++)
			if(rollVal > xcum[i-1] && rollVal <= xcum[i])
				return i;
		
		return -1; // Error, cannot be 
	}
}



public virtual class GenericLocationPlan<T> : IPlanExecutor
{
	Actor actor;
	Dictionary<T, float> candidateWeightsDict;
	float _totalWeights;


	public GenericLocationPlan(Actor actor)
	{
		this.actor = actor;
	}
	
	public virtual List<T> IdentifyCandidates<T>()
	{
		// Find all lairs
		
	}

	public float GetProbability<T>(Object candidate)
	{
		return candidateWeightsDict[candidate]/_totalWeights;		
	}
	
	public Object SelectCandidateRandomly()
	{		
		List<T> towns = IdentifyCandidates<T>();
		
		//Dictionary<Town, float> townProbability = new Dictionary<Town, float>();
		float[] probs = new float[towns.Count];
		for(int i =0; i < towns.Count; i++){
			probs[i] = GetProbability(towns[i]);
		}
		
		int roll = MyStatistics.RandomWeightedIndex(probs);
		return towns[roll];
	}
	
	public virtual void Execute()
	{}
	
	public virtual void Stop()
	{}
}
