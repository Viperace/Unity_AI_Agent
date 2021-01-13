using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Wander : IAgentAction
{
	Actor actor;
	Vector3 midPoint;
	float totalDuration; // how long to entire wandering action
	float radius;
	float idleDuration;     // hhow long before change dest
	float idleDurationVar;   //variance
	NavMeshAgent navMeshAgent;
	float _idleCooldown;
	float _totalCooldown;

	public bool IsDone { get; private set; }
	public bool IsStarted { get; private set; }

	System.Action onCompleteFunc, onFailureFunc;

	public Wander() { }

	public Wander(Actor actor, Vector3 midPoint, float radius, float totalDuration, 
		float idleDuration = 4, float idleDurationVar = 5)
	{
		this.actor = actor;
		this.midPoint = midPoint;
		this.radius = radius;
		this.totalDuration = totalDuration;
		this.idleDuration = idleDuration;
		this.idleDurationVar = idleDurationVar;

		navMeshAgent = actor.GetComponent<NavMeshAgent>();
		IsDone = false;
	}


	public void AddOnCompleteFunc(Action onCompleteFunc)
    {
		this.onCompleteFunc += onCompleteFunc;
	}

	public void AddOnFailureFunc(Action onFailureFunc)
    {
		this.onFailureFunc += onFailureFunc;
	}

	public bool CheckFailure()
    {
		// Nothing to chk
		return false;
	}

    public bool CheckIsCompleted()
    {
		return _totalCooldown < 0;
		
	}

    public void OnComplete()
	{ 
		if(onCompleteFunc != null)
			onCompleteFunc();
	}

    public void OnFailure()
    {
		Debug.Log(this.actor + " fail to wander");
		if(onFailureFunc != null)
			onFailureFunc();
    }

    public void Run()
    {
		IsStarted = true;

		_totalCooldown = totalDuration;
		_idleCooldown = 0;
	}

    public void Update()
    {
		if(IsStarted & !IsDone)
        {
			if(CheckIsCompleted())
            {
				IsDone = true;
				OnComplete();
            }
			
			if(_idleCooldown < 0)
            {
				_idleCooldown = idleDuration + UnityEngine.Random.Range(0, idleDurationVar);

				_MoveToRandomPos();
			}

			_totalCooldown -= Time.deltaTime;
			_idleCooldown -= Time.deltaTime;
		}
    }

	void _MoveToRandomPos()
    {
		Vector3 newPos = new Vector3(Random.Range(-radius, radius), 0, 
										Random.Range(-radius, radius));
		newPos += midPoint;

		bool isValid = navMeshAgent.SetDestination(newPos);
        if (!isValid)
        {
			IsDone = true;
			onFailureFunc();
        }
	}
}
