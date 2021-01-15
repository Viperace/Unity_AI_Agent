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
	public float GetProbability(object candidate);	
	public void Execute(); 
	public void Stop();
}

// Lairs
public class ExecuteGoTown: IPlanExecutor
{
	Actor actor;
	
	public ExecuteGoTown(Actor actor)
	{
		this.actor = actor;		
	}
	
	public List<Town> IdentifyCandidates<Town>()
	{
		// Find all towns within game
		return null;
	}
	
	public float GetProbability(object candidate)
	{
		// Convert var to GameObject
		
		// Find distance between actor and this 
		
		// 1/dist 
		return 0;
	}
	
	public void Execute()
	{
		// Initialize the plan 
		//actor.GoTown
	}
	
	public void Stop() 
	{	
		actor.SetCurrentAction(null);
	}
}
