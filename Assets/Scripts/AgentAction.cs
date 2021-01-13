using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public interface IAgentAction
{
	public void Run();
	public bool CheckIsCompleted();
	public void OnComplete();
	public bool CheckFailure();
	public void OnFailure();
	public void AddOnCompleteFunc(System.Action onCompleteFunc);
	public void AddOnFailureFunc(System.Action onFailureFunc);

	public void Update();
}

/// <summary>
/// Sequence of action chained 1 by 1. 
/// First action complete OR failure.. Move on to next
/// </summary>
public class ActionSequence
{
	Actor actor;
	IAgentAction[] actions;
	public bool IsComplete { get; private set; }

	int numberOfCompletedAction = 0;

	public ActionSequence() { }

	public ActionSequence(Actor actor, params IAgentAction[] actions) 
	{
		this.actor = actor;
		this.actions = actions;
		this.IsComplete = false;
	}

	public void Run()
    {
		ChainTheActions();

		// Start the first one
		if(actions.Length > 0)
        {
			actor.SetCurrentAction(actions[0]);
			actions[0].Run();
		}
			
	}

	// Loop onComplete of one action to the next
	void ChainTheActions()
	{
		for(int i = 0; i < actions.Length - 1; i++)
        {
			IAgentAction act = actions[i];
			IAgentAction nextAct = actions[i + 1];
			act.AddOnCompleteFunc( () => actor.SetCurrentAction(nextAct));
			act.AddOnCompleteFunc(nextAct.Run );
			act.AddOnCompleteFunc( () => this.numberOfCompletedAction++ );
		}
	}


	public int NumberOfRemainingAction()
	{
		int n = 0;
		foreach (IAgentAction act in actions)
        {
			if (!act.CheckIsCompleted())
				n++;
		}

		return n;
    }
}

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
    }

	float _pathRecalcCooldown = 0;
	float _checkCompleteCooldown = 1;
	float _checkFailureCooldown = 2;
	public void Update()
	{
		if (IsStarted & !IsDone)
		{
			if (_pathRecalcCooldown < 0 | navMeshAgent.remainingDistance < 0.1f) // Recalculate path to target, if cooldown done OR reach destination
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

    public void AddOnCompleteFunc(System.Action onCompleteFunc)
    {
		this.onCompleteFunc += onCompleteFunc;
	}

    public void AddOnFailureFunc(Action onFailureFunc)
    {
		this.onFailureFunc += onFailureFunc;
	}
}

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

/// <summary>
/// Request target to participate in combat. If accepted, target/initiaotor will move toward each other,
/// once within attack range, each will begin attack animation.
/// </summary>
public class RequestCombat : IAgentAction
{
	Combatant initiator;
	Combatant target;

	public RequestCombat() { }
	public RequestCombat(Combatant initiator, Combatant target) 
	{
		this.initiator = initiator;
		this.target = target;

	}

	public void AddOnCompleteFunc(Action onCompleteFunc)
    {
		//TODO: Win animation
        throw new NotImplementedException();
    }

    public void AddOnFailureFunc(Action onFailureFunc)
    {
        throw new NotImplementedException();
    }

    public bool CheckFailure()
    {
        throw new NotImplementedException();
    }

    public bool CheckIsCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnComplete()
    {
        throw new NotImplementedException();
    }

    public void OnFailure()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}