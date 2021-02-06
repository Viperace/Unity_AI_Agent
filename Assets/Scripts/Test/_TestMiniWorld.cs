using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestMiniWorld : MonoBehaviour
{
    [SerializeField] int totalCreepsInLairs = 0;
    [SerializeField] int totalHeros = 0;

    //public int creepsLimit = 20;
    public int herosLimit = 10;

    public GameObject monsterPrefab;
    public GameObject heroPrefab;

    void Start()
    {
    }

    int TallyTotalMonster()
    {
        int count = 0;
        foreach(Actor act in FindObjectsOfType<Actor>())
            if (act.CompareTag("Creeps"))
                count++;

        return count;
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
        int n = Lair.lairs.Count;
        List<Lair> lairList = new List<Lair>(Lair.lairs);
        Lair rolledLair = lairList[Random.Range(0, n)];
        rolledLair.SpawnCreeps();
        rolledLair.SpawnCreeps();
        rolledLair.SpawnCreeps();        
    }

    void SpawnRandomHero()
    {
        if (Town.towns != null)
        {
            // Find location to spawn near town
            List<Town> allTowns = new List<Town>(Town.towns);
            Town rolledTown = allTowns[Random.Range(0, allTowns.Count)];
            Vector3 rolledPos = rolledTown.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

            // Instantiate
            GameObject heroGO = Instantiate(heroPrefab, rolledPos, Quaternion.identity);
            heroGO.GetComponent<Brain>().Restart();
        }
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

            _spawnCooldown = 1f;
        }
        _spawnCooldown -= Time.deltaTime;
    }
}
