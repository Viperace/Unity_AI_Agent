using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItem : MonoBehaviour
{
    static LoadItem _instance;

    GearDataArray allgears;
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
        allgears = JsonUtility.FromJson<GearDataArray>(jsonFile.text);
    }


    public BasicGear Spawn(string name)
    {
        foreach (GearJsonData x in allgears.gears)
            if (x.name == name)
                return GearJsonData.Spawn(x);
        return null;
    }

    public BasicGear SpawnRandomItem()
    {
        int roll = Random.Range(0, allgears.gears.Length);
        return GearJsonData.Spawn(allgears.gears[roll]);
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
    public string name;
    public string weaponOrArmor;
    public string type;
    public string carryHand;
    public string singleOrTwoHanded;
    public string grade;
    public string specialName;
    public int level;
    public int star;
    public int starCap;
    public int baseAttack;
    public int baseDefend;
    public float attackMod;
    public float defendMod;
    public int durabilityCap;
    public int basePrice;

    public static BasicGear Spawn(GearJsonData data)
    {
        BasicGear basicGear = new BasicGear();

        // Fill in 
        basicGear.name = data.name;
        basicGear.rarity = Rarity.COMMON;
        basicGear.attack = data.baseAttack;
        basicGear.defend = data.baseDefend;
        basicGear.attackBonus = data.attackMod;
        basicGear.defendBonus = data.defendMod;
        basicGear.durability = data.durabilityCap;

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

}
