using System;
using UnityEngine;
using UnityEngine.AI;


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
