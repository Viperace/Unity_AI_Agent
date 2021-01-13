using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : GenericLocation
{
    [SerializeField] float minQueueDistance = 1;
    [SerializeField] float maxQueueDistance = 3;

    // Put actor to stand at random location, with lower index closer.
    public override IEnumerator TrapActorForDurationCoroutine(Actor actor, float duration)
    {
        // TODO: teleport to queue pos
        Vector3 queueLoc = RandomizeAQueuePoint();
        actor.transform.position = queueLoc;

        // SLow look
        SlowLookAt lookat = actor.GetComponent<SlowLookAt>();
        if (lookat)
            lookat.InitiateLookAt(this.transform);

        // Do nothing
        actor.StandAndWait();
        yield return new WaitForSeconds(duration);

        // Remove the coroutine list
        if (actorCoroutines.ContainsKey(actor))
            actorCoroutines.Remove(actor);

        Release(actor);
    }

    Vector3 RandomizeAQueuePoint()
    {
        float range = Mathf.Min(minQueueDistance + actors.Count * 0.2f, maxQueueDistance); // at most 
        Vector3 mid = this.transform.position;

        float dx = Random.Range(-range, range);
        float dz = Random.Range(-range, range);
        return new Vector3(mid.x + dx, mid.y, mid.z + dz);
    }
}
