using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	Actor actor;
	PlanManager planManager;
	FatigueLevel fatigueLevel;
	NeedsBehavior needsBehavior;
	Utility utility;
	float _reevalPeriod = 3f;
	IEnumerator coroutine;
	void Start()
	{
		this.actor = GetComponent<Actor>();
		this.planManager = new PlanManager(this.actor);
		this.needsBehavior = GetComponent<NeedsBehavior>();
		utility = new Utility();
		fatigueLevel = new FatigueLevel();

		// Init
		Restart();
	}

	// To use if this GO is inactive
	public void Restart()
	{
		// Reset all actions
		if (actor)
			actor.StopAllActions();
		
		// Define coroutine thinking
		coroutine = _ThinkAndDoCoroutine(_reevalPeriod);
		StartCoroutine(coroutine);

		// Reset to idle first
		if (planManager != null) planManager.SetAndExecutePlan(HighLevelPlan.Idle);
	}
	void Update()
	{
	}

	void OnEnable()
    {
		Restart();
    }

	IEnumerator _ThinkAndDoCoroutine(float reevalPeriod, float firstStartDelay = 1f)
    {
		yield return new WaitForSeconds(firstStartDelay);

		while (planManager != null)
		{
			// Eval plan
			float bestScore;
			HighLevelPlan bestPlan = planManager.EvaluateBestPlan(utility, needsBehavior.needs, out bestScore);

			// Change of  plan if this new plan is so much better
			//if (planManager.currentPlan != bestPlan & bestScore > 20f) 
			bool nothingBetterToDo = planManager.currentPlan == HighLevelPlan.Idle;
			bool hasSubOptimalPlan = (planManager.currentPlan != bestPlan & bestScore > 20f);
			if (nothingBetterToDo || hasSubOptimalPlan)
                if (fatigueLevel.ShouldIRest())
                {
					planManager.SetAndExecutePlan(HighLevelPlan.Idle);
					fatigueLevel.Reset();
				}
				else
                {
					planManager.SetAndExecutePlan(bestPlan);
					fatigueLevel.AddFatigue();
					//Debug.Log(actor.gameObject + " has New plan: " + bestPlan + " . Score:" + bestScore);
				}

			yield return new WaitForSeconds(reevalPeriod);
		}
	}

	public HighLevelPlan CurrentPlan { get { return planManager.currentPlan; } }
}


class FatigueLevel
{
	int numberOfPlanCarriedOut;
	public FatigueLevel() { }
	public void AddFatigue() => numberOfPlanCarriedOut++;
  
	public void Reset()
    {
		numberOfPlanCarriedOut = 0;
    }
	public bool ShouldIRest()
    {
		float roll = Random.value;
		if (numberOfPlanCarriedOut > 5)
			return roll > 0;
		else if (numberOfPlanCarriedOut > 4)
			return roll > 0.1f;
		else if (numberOfPlanCarriedOut > 3)
			return roll > 0.15f;
		else if (numberOfPlanCarriedOut > 2)
			return roll > 0.2f;
		else
			return false;
	}
}