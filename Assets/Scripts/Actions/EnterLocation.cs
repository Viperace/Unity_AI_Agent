using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnterLocation : IAgentAction
{
	protected Actor agent;
	protected ILocation town;
	protected float stayDuration; // how long to stay in town

	protected System.Action onCompleteFunc, onFailureFunc;

	public bool IsDone { get; private set; }
	public bool IsStarted { get; private set; }

	public EnterLocation(Actor agent, ILocation town, float stayDuration, System.Action onCompleteFunc = null, System.Action onFailureFunc = null)
	{
		IsDone = false;
		IsStarted = false;
		this.agent = agent;
		this.town = town;
		this.stayDuration = stayDuration;
		this.onCompleteFunc = onCompleteFunc;
	}

	public void Run()
	{
		IsStarted = true;

		bool status = town.IsAdmitable(agent);			
		if (status) 
		{
			IsDone = true;
			town.Admit(agent, stayDuration);
			OnComplete();
		}
        else
        {
			IsDone = true;
			OnFailure();
		}		
	}

	public bool CheckIsCompleted()
	{
		return false;
	}

	public void OnComplete()
	{
		if (onCompleteFunc != null)
			onCompleteFunc();
	}

	
	public void Update()
	{
		
	}

	public void Stop()
	{
		
	}

	public bool CheckFailure()
	{
		//if (town == null || !town.gameObject.activeInHierarchy) // Target gone
		if (town == null ) // Target gone
		{
			Debug.Log("Location gone");
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

	public void AddOnCompleteFunc(System.Action onCompleteFunc)
	{
		this.onCompleteFunc += onCompleteFunc;
	}

	public void AddOnFailureFunc(Action onFailureFunc)
	{
		this.onFailureFunc += onFailureFunc;
	}
}
