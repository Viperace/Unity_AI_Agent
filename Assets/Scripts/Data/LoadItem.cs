using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItem : MonoBehaviour
{
    static LoadItem _instance;

    public GearDataArray gearsData { get; private set; }
    [SerializeField] List<GameObject> allGearPrefabs;
    public static LoadItem Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<LoadItem>();
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        TextAsset jsonFile = Resources.Load("items") as TextAsset;
        gearsData = JsonUtility.FromJson<GearDataArray>(jsonFile.text);
    }


    public BasicGear Spawn(string name)
    {
        foreach (GearJsonData x in gearsData.gears)
            if (x.name == name)
                return GearJsonData.SpawnGear(x);
        return null;
    }

    public BasicGear SpawnRandomItem()
    {
        int roll = Random.Range(0, gearsData.gears.Length);
        return GearJsonData.SpawnGear(gearsData.gears[roll]);
    }

    // Exact name
    public GameObject GetPrefabByName(string name)
    {
        foreach (GameObject g in allGearPrefabs)
            if (string.Equals(g.name, name, System.StringComparison.OrdinalIgnoreCase))
                return g;

        return null;
    }

}


[System.Serializable]
public class GearDataArray
{
    public GearJsonData[] gears;
}

[System.Serializable]
public class GearJsonData
{
    // These variable must follow exactly what is defined in Json
    public string name;
    public string weaponOrArmor;
    public string type;
    public string Subtype;
    public string carryHand;
    public string singleOrTwoHanded;
    public string grade;
    public string specialName;
    public int level;
    public int star;
    public int starCap;
    // Cost
    public int Ore;
    public int Steel;
    public int Coal;
    public int Orichalcum;
    // stat
    public int baseAttack;
    public int baseDefend;
    public float attackMod;
    public float defendMod;
    public int durabilityCap;
    public int basePrice;
    public string PrefabFile;

    public static BasicGear SpawnGear(GearJsonData data)
    {
        BasicGear basicGear = new BasicGear();

        // Fill in 
        basicGear.name = data.name;
        basicGear.rarity = stringToRarity(data.grade);
        basicGear.attack = data.baseAttack;
        basicGear.defend = data.baseDefend;
        basicGear.attackBonus = data.attackMod;
        basicGear.defendBonus = data.defendMod;
        basicGear.durability = data.durabilityCap;
        basicGear.prefabName = data.PrefabFile;
        basicGear.type = data.type;
        basicGear.subtype = data.Subtype;

        if (data.weaponOrArmor == "Weapon")
        {
            if (data.carryHand == "R")
                basicGear.slot = EquipmentSlot.RIGHT_HAND;
            else if(data.carryHand == "L")
                basicGear.slot = EquipmentSlot.LEFT_HAND;
            else
            {
                Debug.Log("Unknown weapon hand");
                basicGear.slot = EquipmentSlot.RIGHT_HAND;
            }
        }
        else
        {
            if (data.type == "Armor")
                basicGear.slot = EquipmentSlot.BODY;
            else if(data.type == "Wrist")
                basicGear.slot = EquipmentSlot.ARM;
            else if (data.type == "Helmet")
                basicGear.slot = EquipmentSlot.HEAD;
        }

        return basicGear;
    }

    public static Blueprint SpawnBlueprint(GearJsonData data)
    {
        BasicGear gear = GearJsonData.SpawnGear(data);
        
        Blueprint blueprint = new Blueprint(gear, data.Ore, data.Steel, data.Coal, data.Orichalcum, data.PrefabFile);

        return blueprint;
    }

    public static Rarity stringToRarity(string x)
    {
        int numOfRarity = System.Enum.GetNames(typeof(Rarity)).Length;
        for (int i = 0; i < numOfRarity; i++)
        {
            Rarity rarity = (Rarity)i;
            if (x.ToUpper().Equals(rarity.ToString()))
                return rarity;
        }

        return Rarity.COMMON;
    }
}
