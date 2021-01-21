using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestMiniWorld : MonoBehaviour
{
    [SerializeField] int totalCreepsInLairs = 0;
    [SerializeField] int totalHeros = 0;

    public int creepsLimit = 20;
    public int herosLimit = 10;

    public GameObject monsterPrefab;
    public GameObject heroPrefab;

    void Start()
    {
    }

    int TallyTotalMonster()
    {
        GameObject[] creeps = GameObject.FindGameObjectsWithTag("Creeps");
        if (creeps != null)
            return creeps.Length;
        else
            return 0;
    }

    int TallyTotalHeros()
    {
        Brain[] brains = FindObjectsOfType<Brain>();
        if (brains != null)
            return brains.Length;
        else
            return 0;
    }

    void RandomSpawnMonster()
    {
        Vector3 pos = new Vector3(Random.Range(-35, 35), 0, Random.Range(-35, 35));
        Instantiate(monsterPrefab, pos, Quaternion.identity);
    }

    void SpawnRandomHero()
    {
        Vector3 pos = new Vector3(Random.Range(-35, 35), 0, Random.Range(-35, 35));
        GameObject heroGO = Instantiate(heroPrefab, pos, Quaternion.identity);
        heroGO.GetComponent<Brain>().Restart();
    }

    float _cooldown = 1;
    float _spawnCooldown = 1;
    void Update()
    {        
        if(_cooldown < 0)
        {
            _cooldown = 1f;
            totalCreepsInLairs = TallyTotalMonster();
            totalHeros = TallyTotalHeros();
        }
        _cooldown -= Time.deltaTime;

        // To spawn
        if (_spawnCooldown < 0)
        {
            if (totalHeros < herosLimit)
                SpawnRandomHero();

            if (totalCreepsInLairs < creepsLimit)
                RandomSpawnMonster();

            _spawnCooldown = 1f;
        }
        _spawnCooldown -= Time.deltaTime;
    }
}
