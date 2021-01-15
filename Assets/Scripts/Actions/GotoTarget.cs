using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GotoTarget : IAgentAction
{
	GameObject agent;
	GameObject target;
	float distanceThreshold;
	float pathRecalcPeriod = 5;
	float checkCompletionPeriod = 0.5f;
	System.Action onCompleteFunc, onFailureFunc;

	NavMeshAgent navMeshAgent;

	public bool IsDone {get; private set;}
	public bool IsStarted { get; private set; }

	public GotoTarget(GameObject agent, GameObject target, float distanceThreshold, 
		System.Action onCompleteFunc = null, System.Action onFailureFunc = null)
	{
		IsDone = false;
		IsStarted = false;
		this.agent = agent;
		this.target = target;
		this.distanceThreshold = distanceThreshold;
		this.onCompleteFunc = onCompleteFunc;

		navMeshAgent = agent.GetComponent<NavMeshAgent>();
	}

	public void Run()
    {
		IsStarted = true;
	}

	public bool CheckIsCompleted()
	{
		if(target == null)
			return false;
        else
        {			
			float dist = Vector3.Distance(agent.transform.position, target.transform.position);
			return dist < distanceThreshold;
		}
	}
	
	public void OnComplete()
	{
		if (onCompleteFunc != null)
            onCompleteFunc();

		if(navMeshAgent.enabled && navMeshAgent.gameObject.activeInHierarchy)
			navMeshAgent.destination = navMeshAgent.transform.position;
		Debug.Log("Arrive " + target.name);
    }

	float _pathRecalcCooldown = 0;
	float _checkCompleteCooldown = 1;
	float _checkFailureCooldown = 2;
	public void Update()
	{
		if (IsStarted & !IsDone)
		{
			if (_pathRecalcCooldown < 0 | navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) // Recalculate path to target, if cooldown done OR reach destination
				if (target)
				{
					// Set target destination
					navMeshAgent.destination = target.transform.position;
					_pathRecalcCooldown = pathRecalcPeriod;
				}

			// Do check Complete
			if (_checkCompleteCooldown < 0)
			{	
				if (CheckIsCompleted())
				{
					IsDone = true;
					OnComplete();
				}
				else
					_checkCompleteCooldown = checkCompletionPeriod;
			}

			// Check failure
			if(_checkFailureCooldown < 0)
				if (CheckFailure())
					OnFailure();

			// Update
			_pathRecalcCooldown -= Time.deltaTime;
			_checkCompleteCooldown -= Time.deltaTime;
			_checkFailureCooldown -= Time.deltaTime;
		}
	}

	public void Stop()
	{
		// Force stop
		navMeshAgent.SetDestination(navMeshAgent.transform.position);
		IsDone = true;
	}

	public void SetPathRecalcPeriod(float dur)
    {
		pathRecalcPeriod = dur;
    }

    public bool CheckFailure()
    {
		if (target == null || !target.gameObject.activeInHierarchy) // Target gone
        {
			Debug.Log("Target gone");
			return true;
		}			

		if (navMeshAgent == null | !navMeshAgent.isActiveAndEnabled)
		{
			Debug.Log("navmesh gone");
			return true;
		}

		//if (navMeshAgent.remainingDistance == Mathf.Infinity) // Cant reach
		//{
		//Debug.Log("Unreachable");
		//return true;
		//}

		return false;
    }

    public void OnFailure()
    {
		Debug.Log(agent + " fail to perform action" + this + ". Canceling");
		IsDone = true;

		if (onFailureFunc != null)
			onFailureFunc();
	}

    public void AddOnCompleteFunc(System.Action onCompleteFunc) => this.onCompleteFunc += onCompleteFunc;
    
    public void AddOnFailureFunc(Action onFailureFunc)
    {
		this.onFailureFunc += onFailureFunc;
	}
}
