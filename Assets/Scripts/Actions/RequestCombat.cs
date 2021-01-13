using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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