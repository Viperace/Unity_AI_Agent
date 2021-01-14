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

*/
public class PlanManager
{
	Actor actor;

	// Dictionary to show completing this action yield how much value.	
	Dictionary<HighLevelPlan, Needs> planBaseScoreDict = new Dictionary<HighLevelPlan, Needs>();

	// Dictionary/Method to estimate 'expectedTimeToComplete'


	public PlanManager() => _InitDictionary();

	void _InitDictionary()
	{
		Needs needAdd;

		needAdd = new Needs();
		planBaseScoreDict.Add(HighLevelPlan.Idle, needAdd);

		needAdd = new Needs();
		needAdd.Energy = 100;
		needAdd.Shopping = 10;
		//needAdd.Socializing = 10;
		planBaseScoreDict.Add(HighLevelPlan.GoTownAndSleep, needAdd);

		needAdd = new Needs();
		needAdd.Energy = 30;
		needAdd.Shopping = -10;
		planBaseScoreDict.Add(HighLevelPlan.EnactTentAndSleep, needAdd);

		needAdd = new Needs();
		needAdd.Shopping = 35;
		planBaseScoreDict.Add(HighLevelPlan.GoShopping, needAdd);

		needAdd = new Needs();
		needAdd.Survival = 100;
		planBaseScoreDict.Add(HighLevelPlan.FleeFromDanger, needAdd);

		needAdd = new Needs();
		needAdd.BloodLust = 10;
		planBaseScoreDict.Add(HighLevelPlan.PatrolAreaForCreep, needAdd);

		needAdd = new Needs();
		needAdd.BloodLust = 30;
		needAdd.Shopping = -10;
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

	public HighLevelPlan EvaluateBestPlan(Utility utility, Needs currentNeeds)
	{
		HighLevelPlan bestPlan = HighLevelPlan.Idle;
		float bestScore = -1000000;
		foreach (HighLevelPlan plan in planBaseScoreDict.Keys)
		{
			// How much needs added for this plan 
			Needs needsAdded = GetBaseNeeds(plan);

			// Convert the needs to marginal utility score
			float score = utility.MarginalScore(currentNeeds, needsAdded);
			if (score > bestScore)
			{
				bestPlan = plan;
				bestScore = score;
			}
		}

		Debug.Log("Best plan " + bestPlan + ": " + bestScore);
		return bestPlan;
	}

	public ActionSequence ConvertPlanToActions(HighLevelPlan plan)
	{
		ActionSequence actions = new ActionSequence();
		switch (plan)
		{
			case HighLevelPlan.Idle:
				//Wander for random duration
				//actions = new ActionSequence(actor, ?? DOIDLE ??);
				break;
			case HighLevelPlan.GoTownAndSleep:
				// Get all valid towns. Random select one 
				//actions = new ActionSequence(actor, ?? GO RANDOM TOWN ??);
				break;
			case HighLevelPlan.EnactTentAndSleep:
				break;
			case HighLevelPlan.GoShopping:
				// Get player 
				//actions = new ActionSequence(actor, ?? GO RANDOM MERCHANT ??);
				break;
			case HighLevelPlan.PatrolAreaForCreep:
				// Get all lairs. Filter for valid one.
				// Random select one
				//actions = new ActionSequence(actor, ?? GO RANDOM LAIR ??);
				break;
			case HighLevelPlan.HuntCreep:
				// Get all monster within range. Filter valid one. 
				// Random select one				
				break;
		}

		return actions;
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