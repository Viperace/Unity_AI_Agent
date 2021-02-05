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
    public List<BasicGear> stashedItems { get; private set; }
    public List<BasicGear> itemsOnSales { get; private set; }

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

        _DebugAddItems();
    }

    void _DebugAddItems()
    {
        BasicGear x1 = new BasicGear("Wooden Sword", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 2, 0);
        BasicGear x2 = new BasicGear("Simple Axe", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 2, 0);
        BasicGear x3 = LoadItem.Instance.Spawn("Shield");
        BasicGear x4 = LoadItem.Instance.Spawn("Sword");
        AddGearToStash(x1, x2, x3, x4);
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
        }

        return 0;
    }
}
