using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lair : MonoBehaviour
{
    public float enterRadius = 3f;
    [SerializeField] GameObject creepPrefab;
    HashSet<Creeps> creeps;

    void Start()
    {
        creeps = new HashSet<Creeps>();
    }

    public void SpawnCreeps()
    {
        // Find spanw point
        float theta = Random.Range(0, 360f);
        Vector3 pivot = this.transform.position;
        Vector3 spawnPoint =  Quaternion.Euler(new Vector3(0, theta, 0)) * (new Vector3(0, 0, 2f)) + pivot;

        //Vector3 spawnPoint = this.transform.position +
        //    new Vector3(Random.Range(-3f, 3f), this.transform.position.y, Random.Range(-3f, 3f));
        
        // Instantiate at location
        GameObject go = Instantiate(creepPrefab, spawnPoint, Quaternion.identity);
        Creeps creep = go.GetComponent<Creeps>();

        // Register
        creeps.Add(creep);
        creep.SetHome(this);
    }

    public void Deregister(Creeps creep)
    {
        if(creep)
            creeps.Remove(creep);
    }

    // Summon creeps home
    public void CallbackAllCreeps()
    {
        foreach (Creeps c in creeps)
            c.GoHome();
    }
}
