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
