using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	Actor actor;
	PlanManager planManager;
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
			{
				planManager.SetAndExecutePlan(bestPlan);
				Debug.Log(actor.gameObject + " has New plan: " + bestPlan + " . Score:" + bestScore);
			}

			yield return new WaitForSeconds(reevalPeriod);
		}
	}

	public HighLevelPlan CurrentPlan { get { return planManager.currentPlan; } }
}
