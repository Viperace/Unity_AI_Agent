using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericLocation : MonoBehaviour, ILocation
{
    [SerializeField] protected float admitRadius = 2f;
    protected HashSet<Actor> actors;
    protected Dictionary<Actor, IEnumerator> actorCoroutines;

    protected void Start()
    {
        actors = new HashSet<Actor>();
        actorCoroutines = new Dictionary<Actor, IEnumerator>();
    }

    public virtual IEnumerator TrapActorForDurationCoroutine(Actor actor, float duration)
    {
        // Stand and stop thinking
        actor.StandAndWait();
        yield return new WaitForSeconds(duration);

        // Remove the coroutine list
        if (actorCoroutines.ContainsKey(actor))
            actorCoroutines.Remove(actor);

        Release(actor);
    }

    public void TrapActorForDuration(Actor actor, float duration)
    {
        IEnumerator coroutine = TrapActorForDurationCoroutine(actor, duration);
        StartCoroutine(coroutine);

        actorCoroutines.Add(actor, coroutine);
    }

    public virtual void Release(Actor actor)
    {
        actor.StopAllActions();
    }

    public float AdmitRadius { get { return admitRadius; } }

    public bool IsAdmitable(Actor actor)
    {
        float dist = Vector3.Distance(actor.transform.position, this.transform.position);
        return dist <= admitRadius;
    }

    public void Admit(Actor actor, float duration)
    {
        // Add
        actors.Add(actor);

        // Make it sleep
        TrapActorForDuration(actor, duration);
    }

}


public class Town : GenericLocation
{
    public static HashSet<Town> towns;
    RestAtTownEffect effect = new RestAtTownEffect();

    protected new void Start()
    {
        base.Start();
        RegisterSelf();
    }

    void RegisterSelf()
    {
        if (towns == null)
            towns = new HashSet<Town>();
        towns.Add(this);
        towns.Remove(null);
    }

    public override IEnumerator TrapActorForDurationCoroutine(Actor actor, float duration)
    {
        actor.gameObject.SetActive(false);
        yield return new WaitForSeconds(duration);

        // Remove the coroutine list
        if (base.actorCoroutines.ContainsKey(actor))
            base.actorCoroutines.Remove(actor);

        Release(actor);
    }

    public override void Release(Actor actor)
    {
        if (!actor.gameObject.activeInHierarchy)
        {
            // Wake
            actor.gameObject.SetActive(true);

            // Eject at random loc
            actor.GetComponentInParent<Transform>().position = this.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));

            // Add bonus
            NeedsBehavior needsBehavior = actor.GetComponent<NeedsBehavior>();
            if (needsBehavior)
                effect.Apply(needsBehavior);
        }
    }

}
