using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
	public static HashSet<Actor> actors;

	protected IAgentAction currentAction;

	protected ActionSequence currentActionSequence;

    void Start()
    {
		// Register		
		if (actors == null)
			actors = new HashSet<Actor>();
		actors.Add(this);
		actors.Remove(null);
	}
    void OnDestroy()
    {
		if(actors!=null)
			actors.Remove(this);
    }
    public void SetCurrentAction(IAgentAction act) => this.currentAction = act;    
	public void SetCurrentActionSequence(ActionSequence seq	) => this.currentActionSequence = seq;
	public void StopAllActions()
    {
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
		if (navMeshAgent)
			navMeshAgent.SetDestination(this.transform.position);

		SetCurrentAction(null);
		SetCurrentActionSequence(null);
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

		currentActionSequence = new ActionSequence(this, goTown, enterTown);
		currentActionSequence.Run();
	}

	public void GotoShopAndShopping(Merchant shop, float stayDuration)
	{
		IAgentAction goShop = new GotoTarget(this.gameObject, shop.gameObject, shop.AdmitRadius * 0.5f);
		IAgentAction startShopping = new EnterLocation(this, shop, stayDuration);

		currentActionSequence = new ActionSequence(this, goShop, startShopping);
		currentActionSequence.Run();
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
		PatrolWaypointsLoop(maxChaseDistance, 1, waypoints);
	}

	public void PatrolWaypointsLoop(float maxChaseDistance, int numberOfLoop, params Vector3[] waypoints)
	{
		PatrolWaypointsLoop(maxChaseDistance, numberOfLoop, null, waypoints);
	}

	public void PatrolWaypointsLoop(float maxChaseDistance, int numberOfLoop, System.Action onCompleteAct, params Vector3[] waypoints)
	{
		Combatant combatant = this.GetComponent<Combatant>();
		if (combatant)
		{
			currentActionSequence = new ActionSequence(this);
			for (int i = 0; i < numberOfLoop; i++)
				foreach (Vector3 pos in waypoints)
				{
					AggresiveGoto patrol = new AggresiveGoto(combatant, pos, maxChaseDistance);
					currentActionSequence.Add(patrol);
				}

			//Attach oncomplete action
			currentActionSequence.SetOnComplete(onCompleteAct);
			currentActionSequence.Run();
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
		this.enabled = false;

		// Do dead animation and Off		
		StartCoroutine(DeathAnimation());
	}

	IEnumerator DeathAnimation()
    {		
		// Do Rotate to death
		this.transform.Rotate(Vector3.forward, 90);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);

		// Wait 1 sec
		float onSurfaceTime = 5f;
		yield return new WaitForSeconds(onSurfaceTime);

		// Sink slowly
		float sunkSpeed = 0.05f;		
		float sunkTime = 10f;
		while (sunkTime > 0)
        {			
			this.transform.position -= (3 * Vector3.up) * sunkSpeed * Time.deltaTime;
			sunkTime -= Time.deltaTime;
			yield return null;
        }

		// Do kill
		Destroy(this.gameObject);		
	}

	void Update()
	{
		if(currentAction != null)
			currentAction.Update();
	}
}

