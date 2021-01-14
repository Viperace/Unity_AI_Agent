using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	Actor actor;
	PlanManager planManager;
	NeedsBehavior needsBehavior;
	Utility utility;

	void Start()
	{
		this.actor = GetComponent<Actor>();
		this.planManager = new PlanManager();
		this.needsBehavior = GetComponent<NeedsBehavior>();
		utility = new Utility();
	}

	void Update()
	{
		
	}

	IEnumerator _ThinkAndDoCoroutine()
    {
		while (true)
		{
			// Eval plan
			HighLevelPlan bestPlan = planManager.EvaluateBestPlan(utility, needsBehavior.needs);

			// Do plan 
			ExecutePlan(bestPlan);

			yield return new WaitForSeconds(3f);
		}
	}


	void ExecutePlan(HighLevelPlan plan)
	{
		//actor.DoSequence(plan);
	}
}
