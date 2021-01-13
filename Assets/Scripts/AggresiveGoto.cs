
/* Goto destination, but if see any suitable target, will trigger ChaseAndAttack

*/
public class AggresiveGoto : IAgentAction
{
	Combatant agent;
	Vector3 origin;		
	Vector3 destination;
	float maxChaseDistance; // Agent must keep within this distance from straight line drawn from origin to the destination.
	float checkCompletionPeriod = 0.5f;
	float scanTargetPeriod = 1f;
	System.Action onCompleteFunc, onFailureFunc;
	FieldOfView agentFov;

	NavMeshAgent navMeshAgent;
	List<GameObject> _excludedTargets;

	public bool IsDone {get; private set;}
	public bool IsStarted { get; private set; }

	public GotoTarget(Combatant agent, Vector3 destination, float maxChaseDistance = 10,
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
			float dist = Vector3.Distance(agent.transform.position, destination);
			return dist < distanceThreshold;
		}
	}
	
	public void OnComplete()
	{
		if (onCompleteFunc != null)
            onCompleteFunc();
    }
	
	void ResumeGotoAndExcludeThisTarget(Combatant target)
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
					
					System.Action<Combatant> resumeAct = () => ResumeGotoAndExcludeThisTarget(target)

					agent.ChaseAndAttack(target, resumeAct);
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
		foreach(Actor target in agentFov)
		{			 			
			// Check if target is valid and target is within chasing range 
			if( !_excludedTargets.Contains(target) &&
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

    public void AddOnFailureFunc(Action onFailureFunc)
    {
		this.onFailureFunc += onFailureFunc;
	}
}