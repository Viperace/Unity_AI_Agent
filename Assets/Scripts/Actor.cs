using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
	protected IAgentAction currentAction;

    
	public void SetCurrentAction(IAgentAction act)
    {
		this.currentAction = act;
    }

	public void WanderAround(Vector3 pos, float radius = 5, float totalDuration = 30, float idleDuration = 4, float idleDurationVar = 5)
    {
		IAgentAction wander = new Wander(this, pos, radius, totalDuration);
		SetCurrentAction(wander);
		wander.Run();
	}


	public void GotoTownAndEnter(Town town, float stayDuration)
	{
		IAgentAction goTown = new GotoTarget(this.gameObject, town.gameObject, town.AdmitRadius*0.5f);
		IAgentAction enterTown = new EnterLocation(this, town, stayDuration);

		ActionSequence sequence = new ActionSequence(this, goTown, enterTown);
		sequence.Run();
	}

	public void GotoTarget(GameObject target, System.Action onCompleteAction = null, 
		float distanceThreshold = 1.5f)
	{
		System.Action callback = null;
		callback += () => Debug.Log("Arrived " + target);
		callback += () => currentAction = null;
		callback += onCompleteAction;
		currentAction = new GotoTarget(this.gameObject, target, distanceThreshold, callback);
		currentAction.Run();
	}

	public void PatrolWaypoints(float maxChaseDistance, params Vector3[] waypoints)
	{
		Combatant combatant = this.GetComponent<Combatant>();
		if (combatant) 
		{
			ActionSequence sequence = new ActionSequence(this);
			foreach (Vector3 pos in waypoints)
			{
				AggresiveGoto patrol = new AggresiveGoto(combatant, pos, maxChaseDistance);
				sequence.Add(patrol);
			}
			sequence.Run();
		}
	}

	public void StandAndWait()
    {
		//TODO: Actual idle action
		currentAction = null;
	}

	// All death animation and logic
	public void DoDeath()
    {
		// Stop all actions
		if(currentAction != null)
        {
			currentAction.Stop();
			SetCurrentAction(null);
		}

		// Turn off 
		this.GetComponent<NavMeshAgent>().enabled = false;
		this.GetComponent<SlowLookAt>().enabled = false;
		this.GetComponent<Combatant>().enabled = false;
		this.GetComponentInChildren<FieldOfView>().enabled = false;

		// Do dead animation
		this.transform.Rotate(Vector3.forward, 90);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);
		this.enabled = false;

	}

	void Update()
	{
		if(currentAction != null)
			currentAction.Update();
	}
}
