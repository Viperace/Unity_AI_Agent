using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : GenericLocation
{
    [SerializeField] float minQueueDistance = 1;
    [SerializeField] float maxQueueDistance = 3;
    ShoppingEffect effect = new ShoppingEffect();

    [SerializeField] List<BasicGear> gearsForSale;
    [SerializeField] int maxWaresDisplayCapacity = 5;

    ShopAI shopAI;
    public static HashSet<Merchant> shops;

    protected new void Start()
    {
        base.Start();
        shopAI = GetComponent<ShopAI>();
        gearsForSale = new List<BasicGear>();

        RegisterSelf();
    }
    void RegisterSelf()
    {
        if (shops == null)
            shops = new HashSet<Merchant>();
        shops.Add(this);
        shops.Remove(null);
    }

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
        Brain brain = actor.GetComponent<Brain>();
        if (brain) brain.enabled = false;
        yield return new WaitForSeconds(duration);

        // Initiate Shopping 
        List<BasicGear> itemsWanted = actor.DecideShoppingList(this.gearsForSale);
        List<BasicGear> itemsBought = this.PerformSale(actor, itemsWanted);
        actor.TakeItems(itemsBought);
        float satisfaction = itemsBought.Count > 0 ? 1f : 0.4f;

        // Remove the coroutine list
        if (actorCoroutines.ContainsKey(actor))
            actorCoroutines.Remove(actor);

        Release(actor, satisfaction);
    }

    Vector3 RandomizeAQueuePoint()
    {
        float range = Mathf.Min(minQueueDistance + actors.Count * 0.2f, maxQueueDistance); // at most 
        Vector3 mid = this.transform.position;

        float dx = Random.Range(-range, range);
        float dz = Random.Range(-range, range);
        return new Vector3(mid.x + dx, mid.y, mid.z + dz);
    }

    public override void Release(Actor actor) => Debug.Log("This hsould be dead end");

    void Release(Actor actor, float effectSatisfaction)
    {
        base.Release(actor);

        // Add bonus
        NeedsBehavior needsBehavior = actor.GetComponent<NeedsBehavior>();
        if (needsBehavior)
        {
            // FIXME: How to add satisfcation ?
            effect.SetSatisfactionDiscount(effectSatisfaction);
            effect.Apply(needsBehavior);
        }

        Brain brain = actor.GetComponent<Brain>();
        if (brain) brain.enabled = true;
    }

    //public void SetGearsForSale(List<BasicGear> gears) => this.gearsForSale = new List<BasicGear>(gears);
    public void AddGearsForSale(params BasicGear[] gears)
    {
        foreach (BasicGear g in gears)
            if(gearsForSale.Count < maxWaresDisplayCapacity)
                this.gearsForSale.Add(g);
    }

    public void RemoveGearsFromSale(BasicGear gear)
    {
        this.gearsForSale.Remove(gear);
    }


    // Return true if all sale is being computed. Else return false
    public List<BasicGear> PerformSale(Ishopper shopper, List<BasicGear> itemsInCart)
    {
        List<BasicGear> itemsBought = new List<BasicGear>();
        foreach (BasicGear item in itemsInCart)
            if (gearsForSale.Contains(item))
            {
                int price = _SetSalePrice(item);
                if (shopper.Pay(price))
                {
                    gearsForSale.Remove(item);
                    itemsBought.Add(item);
                }
            }

        return itemsBought;
    }

    int _SetSalePrice(BasicGear gear)
    {
        // FIXME
        return 10;
    }

}
