using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocation
{
    public IEnumerator TrapActorForDurationCoroutine(Actor actor, float duration);

    public void TrapActorForDuration(Actor actor, float duration);

    public void Release(Actor actor);

    public void Admit(Actor actor, float duration);

    public bool IsAdmitable(Actor actor);
}
