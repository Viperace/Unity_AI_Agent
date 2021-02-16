using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static PlayerData _instance;
    public static PlayerData Instance { get { return _instance; } }

    public string playerName;
    public int level { get; set; }
    public int gold { get; set; }
    public int ore { get; set; }
    public int steel { get; set; }
    public int orichalcum { get; set; }
    public int coal { get; set; }
    public List<BasicGear> stashedItems { get; private set; }
    public List<BasicGear> itemsOnSales { get; private set; }
    public HashSet<Blueprint> unlockedBlueprints { get; private set; }

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        // Give free money
        gold = 1000;
        ore = 50;
        steel = 10;
        orichalcum = 1;

        // Init
        stashedItems = new List<BasicGear>();
        itemsOnSales = new List<BasicGear>();
        unlockedBlueprints = new HashSet<Blueprint>();

        Invoke("_DebugAddItems", 0.5f);
    }

    void _DebugAddItems()
    {
        // Item
        //BasicGear x1 = new BasicGear("Wooden Sword", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 2, 0);
        //BasicGear x2 = new BasicGear("Simple Axe", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 2, 0);
        //BasicGear x3 = LoadItem.Instance.Spawn("Shield");
        //BasicGear x4 = LoadItem.Instance.Spawn("Sword");
        BasicGear x1 = LoadItem.Instance.Spawn("War Crafter");
        BasicGear x2 = LoadItem.Instance.Spawn("Warhammer");
        AddGearToStash(x1, x2);

        // Blue print
        UnlockBlueprint("Short Sword");
        UnlockBlueprint("Club");
        UnlockBlueprint("Short Staff");
        UnlockBlueprint("Short Bow");
        UnlockBlueprint("Buckler");
        UnlockBlueprint("War Crafter");
        
        UnlockBlueprint("Cutlass");
    }

    public bool UnlockBlueprint(string blueprintName)
    {
        // Validate this blue print is within database 
        if (Blueprint.BlueprintsDictionary.ContainsKey(blueprintName))
        {
            // Validate this has not already exist
            foreach (Blueprint b in unlockedBlueprints)
                if(b.name == blueprintName)
                {
                    Debug.LogWarning("Blueprint already unlocked by player. " + blueprintName);
                    return false;
                }

            // Spoawn and add
            Blueprint blueprint = Blueprint.SpawnBlueprint(blueprintName);
            unlockedBlueprints.Add(blueprint);
            return true;
        }
        else
        {
            Debug.LogWarning("Blueprint does not exist in database. Not added to player. " + blueprintName);
            return false;
        }
    }

    public bool _LockBlueprint(string blueprintName)
    {
        // Validate this blue print is good
        if (Blueprint.BlueprintsDictionary.ContainsKey(blueprintName))
        {
            Blueprint toDelete = null;
            foreach(Blueprint b in unlockedBlueprints)
                if (b.name == blueprintName)
                    toDelete = b;

            unlockedBlueprints.Remove(toDelete);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddGearToStash(params BasicGear[] gears)
    {
        foreach(BasicGear gear in gears)
            if(!stashedItems.Contains(gear) & gear != null)
                stashedItems.Add(gear);
    }

    public void RemoveGearFromStash(params BasicGear[] gears)
    {
        foreach (BasicGear gear in gears)
            if (stashedItems.Contains(gear))
                stashedItems.Remove(gear);
    }

    public void AddCommodity(Commodity commodity, int quantity)
    {
        switch (commodity)
        {
            case Commodity.GOLD:
                gold += quantity;
                break;
            case Commodity.ORE:
                ore += quantity;
                break;
            case Commodity.STEEL:
                steel += quantity;
                break;
            case Commodity.ORICHALCUM:
                orichalcum += quantity;
                break;
            case Commodity.COAL:
                coal += quantity;
                break;
        }
    }

    public int GetCommodityQuantity(Commodity commodity)
    {
        switch (commodity)
        {
            case Commodity.GOLD:
                return gold;
            case Commodity.ORE:
                return ore;
            case Commodity.STEEL:
                return steel;
            case Commodity.ORICHALCUM:
                return orichalcum;
            case Commodity.COAL:
                return coal;
        }

        return 0;
    }
}
