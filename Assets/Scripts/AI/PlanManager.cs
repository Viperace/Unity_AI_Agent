using System.Collections.Generic;
using UnityEngine;


/*
This function list the set of ALL POSSIBLE actions for this agent 
	- EnactTentAndSleep (where)
	- GoTownAndSleep (which?)
	- HuntCreep	(which?)
	- GoShopping (just 1 smith)
	- PatrolAreaForCreep
	- PatrolAreaForSocializing
	- FleeFromDanger

//TODO: When plan done, back to null
*/
public class PlanManager
{
	Actor actor;

	IPlanExecutor planExecutor;

	// Dictionary to show completing this action yield how much value.	
	Dictionary<HighLevelPlan, Needs> planBaseScoreDict = new Dictionary<HighLevelPlan, Needs>();

	public HighLevelPlan currentPlan { get; set; }

	public PlanManager(Actor actor)
	{
		this.actor = actor;
		_InitDictionary();
	}

	void _InitDictionary()
	{
		Needs needAdd = Needs.zero;

		planBaseScoreDict.Add(HighLevelPlan.Idle, needAdd);

		needAdd.Energy = 100;
		needAdd.Shopping = 10;
		//needAdd.Socializing = 10;
		planBaseScoreDict.Add(HighLevelPlan.GoTownAndSleep, needAdd);

		needAdd.Energy = 30;
		//needAdd.Shopping = -10; // No negative effect
		planBaseScoreDict.Add(HighLevelPlan.EnactTentAndSleep, needAdd);

		needAdd.Shopping = 35;
		planBaseScoreDict.Add(HighLevelPlan.GoShopping, needAdd);

		needAdd.HP = 100;
		planBaseScoreDict.Add(HighLevelPlan.FleeFromDanger, needAdd);

		needAdd.BloodLust = 10;
		planBaseScoreDict.Add(HighLevelPlan.PatrolAreaForCreep, needAdd);

		needAdd.BloodLust = 30*0;
		//needAdd.Shopping = -10; // Should not have -ve effect
		planBaseScoreDict.Add(HighLevelPlan.HuntCreep, needAdd);

		// PatrolAreaForSocializing
	}

	// Return how much needs to regenerated if this plan is executed
	public Needs GetBaseNeeds(HighLevelPlan plan)
	{
		if (planBaseScoreDict.ContainsKey(plan))
		{
			Needs needGenerated = planBaseScoreDict[plan];
			return needGenerated;
		}
		else
			return new Needs();
	}

	// FIXME: How to make it hunt creeps 
	public HighLevelPlan EvaluateBestPlan(Utility utility, Needs currentNeeds, out float marginalScore)
	{
		Dictionary<HighLevelPlan, float> planScoreDict = _GetPlanSensitivity(utility, currentNeeds);

		HighLevelPlan bestPlan = HighLevelPlan.Idle;
		float bestScore = -10000;
		foreach (HighLevelPlan plan in planScoreDict.Keys)
		{			
			float score = planScoreDict[plan];
			if (score > bestScore)
			{
				bestPlan = plan;
				bestScore = score;
			}
		}

		marginalScore = bestScore;
		return bestPlan;
	}

	Dictionary<HighLevelPlan, float> _GetPlanSensitivity(Utility utility, Needs currentNeeds)
	{
		Dictionary<HighLevelPlan, float> planScoreDict = new Dictionary<HighLevelPlan, float>();

		foreach (HighLevelPlan plan in planBaseScoreDict.Keys)
		{
			// How much needs added for this plan 
			Needs needsAdded = GetBaseNeeds(plan);

			// Convert the needs to marginal utility score
			float score = utility.MarginalScore(currentNeeds, needsAdded);

			// Save result to dict
			planScoreDict.Add(plan, score);
		}

		return planScoreDict;
	}

	public void ExecutePlan(HighLevelPlan plan)
	{
		switch (plan)
		{
			case HighLevelPlan.Idle:
				actor.WanderAround(actor.transform.position, Random.Range(4f, 8f));
				break;
			case HighLevelPlan.GoTownAndSleep:
				planExecutor = new ExecuteGoTown(this.actor);
				break;
			case HighLevelPlan.EnactTentAndSleep:
				break;
			case HighLevelPlan.GoShopping:
				break;
			case HighLevelPlan.PatrolAreaForCreep:
				planExecutor = new ExecutePatrolForCreeps(this.actor);
				break;
			case HighLevelPlan.HuntCreep:
				// Get all monster within range. Filter valid one. 
				// Random select one				
				break;
		}

		if (planExecutor != null)
			planExecutor.Execute();
	}
}


public enum HighLevelPlan
{
	Idle = 0,
	GoTownAndSleep,
	EnactTentAndSleep,
	GoShopping,
	FleeFromDanger,
	PatrolAreaForCreep,
	HuntCreep
}