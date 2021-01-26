using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lair : MonoBehaviour
{
    public static HashSet<Lair> lairs;
    public float enterRadius = 3f;
    [SerializeField] int creepsCapacity = 5;  // Maximum 'live' creeps it can support
    [SerializeField] float spawnInterval = 10f;
    [SerializeField] float spawnIntervalVariance = 2f;
    [SerializeField] GameObject[] creepPrefabs;
    HashSet<Creeps> creeps;

    void Awake()
    {
        creeps = new HashSet<Creeps>();
        RegisterLair();
    }

    float _spawnCooldown = 0;
    void Update()
    {
        _spawnCooldown -= Time.deltaTime;
        if ( _spawnCooldown < 0 && CreepsCount < creepsCapacity )
        {
            SpawnCreeps();
            _spawnCooldown = spawnInterval + Random.Range(0, spawnIntervalVariance);
        }

    }

    void RegisterLair()
    {
        if (lairs == null)
            lairs = new HashSet<Lair>();
        lairs.Add(this);
        lairs.Remove(null);
    }

    public void SpawnCreeps()
    {
        // Find spanw point
        float theta = Random.Range(0, 360f);
        Vector3 pivot = this.transform.position;
        Vector3 spawnPoint =  Quaternion.Euler(new Vector3(0, theta, 0)) * (new Vector3(0, 0, 2f)) + pivot;

        // Random roll creep prefab to spawn
        int roll = Random.Range(0, creepPrefabs.Length);        

        // Instantiate at location
        GameObject go = Instantiate(creepPrefabs[roll], spawnPoint, Quaternion.identity);
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
        FlushDeadCreeps();

        foreach (Creeps c in creeps)
            c.GoHome();
    }

    public float AverageLevel
    {
        get
        {
            float avgLevel = 0;
            foreach(GameObject go in creepPrefabs)
            {
                Creeps creep = go.GetComponent<Creeps>();
                if (creep)
                    avgLevel += creep.Level;
            }
            avgLevel /= creepPrefabs.Length;
            return avgLevel;
        }
    }

    public float AverageCreepsAttack
    {
        get
        {
            float avgAttack = 0;
            int n = 0;
            foreach (GameObject go in creepPrefabs)
            {
                CombatStat creep = go.GetComponent<CombatStat>();
                if (creep)
                {
                    avgAttack += creep.AttackPower();
                    n++;
                }
            }
            avgAttack /= n;
            return avgAttack;
        }
    }

    public int CreepsCount 
    { 
        get 
        {
            FlushDeadCreeps();
            return creeps.Count;
        } 
    }

    List<Creeps> flushList;
    void FlushDeadCreeps()
    {
        flushList = new List<Creeps>();
        foreach (Creeps c in creeps)
            if (c == null)
                flushList.Add(c);

        for (int i = 0; i < flushList.Count; i++)
            creeps.Remove(flushList[i]);
    }
}
