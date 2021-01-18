using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestMonsterActor : MonoBehaviour
{
    void Start()
    {
        // Find monster
        Actor[] allActors = FindObjectsOfType<Actor>();
        List<Actor> creeps = new List<Actor>();
        foreach (Actor a in allActors)
            if (a.CompareTag("Creeps"))
            {
                creeps.Add(a);
                a.WanderAround(a.transform.position, 8, -1);
            }
    }

    public void _AllAgentPatrol()
    {
        Actor[] allActors = FindObjectsOfType<Actor>();
        List<Actor> heros = new List<Actor>();
        foreach(Actor a in allActors)
        {
            if (a.CompareTag("Hero"))
                heros.Add(a);
        }

        Actor a1 = heros[0];
        a1.PatrolWaypointsLoop(7, 20, a1.transform.position + new Vector3(8, 0, 0), a1.transform.position + new Vector3(-8, 0, 0));

        if(heros.Count == 2)
        {
            Actor a2 = heros[1];
            a2.PatrolWaypointsLoop(7, 20, a2.transform.position + new Vector3(0, 0, 8), a1.transform.position + new Vector3(0, 0, -8));
        }        
    }

    public void _Spawn()
    {
        Lair lair = FindObjectOfType<Lair>();
        lair.SpawnCreeps();
    }

    public void _CallCreepsHome()
    {
        Lair lair = FindObjectOfType<Lair>();
        lair.CallbackAllCreeps();
    }
}
