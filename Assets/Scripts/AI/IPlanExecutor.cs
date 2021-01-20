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
	//public List<T> IdentifyCandidates<T>();

	//public float GetProbability(Object candidate);
	public Object SelectCandidateRandomly();	
	public void Execute();
	public void Update();
	public void Stop();
	public void SetOnPlanComplete(System.Action action);
}
