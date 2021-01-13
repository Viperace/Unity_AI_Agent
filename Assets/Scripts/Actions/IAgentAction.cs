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
