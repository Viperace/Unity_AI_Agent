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

	void Start()
	{
		this.actor = GetComponent<Actor>();
		this.planManager = new PlanManager();
		this.needsBehavior = GetComponent<NeedsBehavior>();
		utility = new Utility();

		// Init thinking
		StartCoroutine(_ThinkAndDoCoroutine(_reevalPeriod));
	}

	void Update()
	{
		
	}

	IEnumerator _ThinkAndDoCoroutine(float reevalPeriod)
    {
		while (true)
		{
			// Eval plan
			float bestScore;
			HighLevelPlan bestPlan = planManager.EvaluateBestPlan(utility, needsBehavior.needs, out bestScore);

			// Do plan
			if(planManager.currentPlan != bestPlan)
            {
				if(bestScore > 10f)
                {
					Debug.Log(actor.gameObject + " has New plan: " + bestPlan + " . Score:" + bestScore);
				}
				else
					Debug.Log(actor.gameObject + " keep current plan");
			}
		
			yield return new WaitForSeconds(reevalPeriod);
		}
	}

}
