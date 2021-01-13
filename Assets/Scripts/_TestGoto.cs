using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestGoto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _GotoTest()
    {
        GameObject a1 = GameObject.Find("Capsule 1");
        GameObject a2 = GameObject.Find("Capsule 2");

        a1.GetComponent<Actor>().GotoTarget(a2);
    }

    public void _GotoTown()
    {
        GameObject a1 = GameObject.Find("Capsule 1");
        Town town = FindObjectOfType<Town>();
        System.Action x = () => Debug.Log("I enter !");

        IAgentAction goTown = new GotoTarget(a1, town.gameObject, 2f);
        IAgentAction enterTown = new EnterLocation(a1.GetComponent<Actor>(), town, Random.Range(1.5f, 3f));

        ActionSequence sequence = new ActionSequence(a1.GetComponent<Actor>(), goTown, enterTown);
        sequence.Run();
    }

    public void _2ndChase1()
    {
        GameObject a1 = GameObject.Find("Capsule 1");
        GameObject a2 = GameObject.Find("Capsule 2");

        a2.GetComponent<Actor>().GotoTarget(a1);
    }

    public void _2ndChase1ThenGoTown()
    {
        GameObject a1 = GameObject.Find("Capsule 1");
        GameObject a2 = GameObject.Find("Capsule 2");
        GameObject town = GameObject.Find("Town");

        //Actor actor = a2.GetComponent<Actor>();
        //System.Action goTown = () => actor.GotoTown(town);
        //actor.GotoTarget(a1, goTown);

        // Use sequence
        Actor actor = a2.GetComponent<Actor>();
        IAgentAction goTown = new GotoTarget(a2, town, 2f);
        IAgentAction goTarget = new GotoTarget(a2, a1, 1.5f);
        IAgentAction goTown2 = new GotoTarget(a2, town, 2f);
        IAgentAction goTarget2 = new GotoTarget(a2, a1, 1.5f);

        ActionSequence sequence = new ActionSequence(actor, goTarget, goTown, goTarget2, goTown2);
        sequence.Run();
    }

    public void _BothWander()
    {
        Actor[] actors = FindObjectsOfType<Actor>();
        foreach(Actor a in actors)
        {
            // Method 1
            a.WanderAround(a.transform.position);
            
            // Raw method
            //IAgentAction wander = new Wander(a, a.transform.position, 5, 30);
            //a.SetCurrentAction(wander);
            //wander.Run();
        }
    }

    public void _1Attack2()
    {
        GameObject a1 = GameObject.Find("Capsule 1");
        GameObject a2 = GameObject.Find("Capsule 2");

        Combatant attacker = a1.GetComponent<Combatant>();
        attacker.ChaseAndAttack(a2.GetComponent<Combatant>());
    }

    public void _GoShopping()
    {
        Actor[] actors = FindObjectsOfType<Actor>();

        Merchant shop = FindObjectOfType<Merchant>();
        foreach (Actor a in actors)
        {
            IAgentAction goShop = new GotoTarget(a.gameObject, shop.gameObject, 1f);
            IAgentAction enterTown = new EnterLocation(a.GetComponent<Actor>(), shop, Random.Range(4f, 7f));

            ActionSequence sequence = new ActionSequence(a.GetComponent<Actor>(), goShop, enterTown);
            sequence.Run();
        }
    }
}
