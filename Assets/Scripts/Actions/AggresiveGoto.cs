using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/* Goto destination, but if see any suitable target, will trigger ChaseAndAttack

*/
public class AggresiveGoto : IAgentAction
{
	Combatant agent;
	Actor actor;
	Vector3 origin;		
	Vector3 destination;
	float destinationDistanceThreshold = 1.5f; 
	float maxChaseDistance; // Agent must keep within this distance from straight line drawn from origin to the destination.
	float checkCompletionPeriod = 0.5f;
	float scanTargetPeriod = 1f;
	System.Action onCompleteFunc, onFailureFunc;
	FieldOfView agentFov;

	NavMeshAgent navMeshAgent;
	List<GameObject> _excludedTargets;

	public bool IsDone {get; private set;}
	public bool IsStarted { get; private set; }

	public AggresiveGoto(Combatant agent, Vector3 destination, float maxChaseDistance = 10,
		System.Action onCompleteFunc = null, System.Action onFailureFunc = null)
	{
		IsDone = false;
		IsStarted = false;
		this.agent = agent;
		this.origin = agent.transform.position;
		this.destination = destination;
		this.maxChaseDistance = maxChaseDistance;
		this.onCompleteFunc = onCompleteFunc;
		this.onFailureFunc = onFailureFunc;

		_excludedTargets = new List<GameObject>();
		navMeshAgent = agent.GetComponent<NavMeshAgent>();
		agentFov = agent.GetComponentInChildren<FieldOfView>();
		actor = agent.GetComponent<Actor>();
	}

	public void Run()
    {
		IsStarted = true;

		// Start
		navMeshAgent.destination = destination;

	}

	public bool CheckIsCompleted()
	{
		float dist = Vector3.Distance(agent.transform.position, destination);
		
		return dist < destinationDistanceThreshold;
	}
	
	public void OnComplete()
	{
		if (onCompleteFunc != null)
            onCompleteFunc();

		// Stop
		navMeshAgent.destination = navMeshAgent.transform.position;
		Debug.Log(actor + " done patrolling.");
    }
	
	void ResumeGotoAndExcludeThisTarget(GameObject target)
	{
		//1. Go back to the patrol path line 
		navMeshAgent.destination = destination;
		
		//2. exclude this target going forward
		_excludedTargets.Add(target);
	}

	float _checkCompleteCooldown = 1; // initialize
	float _checkFailureCooldown = 2; // initialize
	float _scanTargetCooldown = 0.1f; // initialize
	List<Combatant> targets;
	public void Update()
	{
		if (IsStarted & !IsDone)
		{
			// Scanning for enemies
			if(_scanTargetCooldown < 0)
			{
				targets = ScanForValidTargets();
				if(targets.Count > 0) // Chase target 
				{
					Combatant target = targets[0];

					System.Action resumeAct = () => ResumeGotoAndExcludeThisTarget(target.gameObject);

					agent.ChaseAndAttack(target, resumeAct);
					agent.AddOnWinAction(resumeAct);

					Debug.Log("Target found. Chasing!");
				}
				else  // Head toward destination
				{
					navMeshAgent.destination = destination;
				}
				
				_scanTargetCooldown = scanTargetPeriod;
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
			_checkCompleteCooldown -= Time.deltaTime;
			_checkFailureCooldown -= Time.deltaTime;
			_scanTargetCooldown -= Time.deltaTime;
		}
	}


	float DistanceFromTargetToPath(Transform target)
	{
		Vector3 dir = (destination - origin).normalized;
		float dist = Vector3.Cross(dir, target.position - origin).magnitude;
		return dist;
	}
	
	List<Combatant> ScanForValidTargets()
	{
		List<Combatant> targets = new List<Combatant>();
		foreach(Actor target in agentFov.actorsWithinView)
		{			 			
			// Check if target is valid and target is within chasing range 
			if( !_excludedTargets.Contains(target.gameObject) &&
				IsDesiredTarget(target.gameObject) && 				
				DistanceFromTargetToPath(target.transform) < maxChaseDistance )
			{
				Combatant c = target.GetComponent<Combatant>();
				if(c)
					targets.Add(c);
			}
		}
		
		return targets;
	}
	
	// Check if target is to be chased or not 
	protected bool IsDesiredTarget(GameObject target)
	{
		Actor obj = target.GetComponent<Actor>();
		// Check if it is friend or foe
		if(obj)
			return true;
		
		return false;
	}


    public bool CheckFailure()
    {
		// Not sure how can this be failed 
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

    public void AddOnFailureFunc(System.Action onFailureFunc)
    {
		this.onFailureFunc += onFailureFunc;
	}
}