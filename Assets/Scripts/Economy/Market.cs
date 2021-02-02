using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    static Market _instance;
    public static Market Instance { get { return _instance; } }

    Dictionary<Commodity, float> prices = new Dictionary<Commodity, float>();

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        // Init price
        prices.Add(Commodity.GOLD, 1);
        prices.Add(Commodity.ORE, 5);
        prices.Add(Commodity.STEEL, 25);
        prices.Add(Commodity.ORICHALCUM, 500);
    }

    float _stepSize = 60; // in seconds
    float _stepCooldown;
    void Update()
    {
        if(_stepCooldown < 0)
        {
            _stepCooldown = _stepSize;
            SimulatePriceOneStep();
        }
        _stepCooldown -= Time.deltaTime;
    }
    void SimulatePriceOneStep()
    {

    }

    public float GetValue(Commodity commodity)
    {
        return prices[commodity];
    }
}

public enum Commodity
{
    GOLD, ORE, STEEL, ORICHALCUM
}
