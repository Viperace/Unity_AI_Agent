using Superbest_random;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    static Market _instance;
    public static Market Instance { get { return _instance; } }

    [SerializeField] Dictionary<Commodity, float> prices = new Dictionary<Commodity, float>();
    Dictionary<Commodity, float> volatilities = new Dictionary<Commodity, float>();
    Dictionary<Commodity, float> drifts = new Dictionary<Commodity, float>();

    // Number of second to represent 1 day
    [SerializeField] float _stepSize = 60; // in seconds. 
    float _stepCooldown;

    System.Random rng;
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        // Init RNG
        rng = new System.Random();

        // Init price
        prices.Add(Commodity.GOLD, 1);
        prices.Add(Commodity.ORE, 5);
        prices.Add(Commodity.STEEL, 25);
        prices.Add(Commodity.ORICHALCUM, 500);

        // Init drifts
        drifts.Add(Commodity.GOLD, 0);
        drifts.Add(Commodity.ORE, 0);
        drifts.Add(Commodity.STEEL, 0);
        drifts.Add(Commodity.ORICHALCUM, 0);

        // Init vol
        volatilities.Add(Commodity.GOLD, 0);
        volatilities.Add(Commodity.ORE, 0.3f);
        volatilities.Add(Commodity.STEEL, 0.15f);
        volatilities.Add(Commodity.ORICHALCUM, 0.2f);

    }

    void _Test()
    {
        float[,] cor = new float[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                cor[i, j] = 0;

        cor[0, 0] = 1;
        cor[1, 1] = 1;
        cor[2, 2] = 1;
        cor[1, 2] = 0.6f;
        cor[2, 1] = 0.6f;
        for (int i = 0; i < 10; i++)
        {
            float[] test = MyStatistics.GenerateCorrelatedRandomVariables(cor, rng);
            Debug.Log(test[0] + "," + test[1] + "," + test[2]);
        }
    }

    void Update()
    {
        if (_stepCooldown < 0)
        {
            _stepCooldown = _stepSize;
            SimulatePriceOneStep();
        }
        _stepCooldown -= Time.deltaTime;
    }

    void SimulatePriceOneStep()
    {
        /* dS = mu*dt + sig*dW
			S(t)=S(t-1)*exp((mu-0.5*sig^2)*T + sig*sqrt(T)*Z)
			where Z~N(0, 1)
		*/

        foreach (Commodity comm in Enum.GetValues(typeof(Commodity)))
        {
            //Commodity comm = Commodity.ORE;
            float mu = drifts[comm];
            float vol = volatilities[comm];
            float S0 = prices[comm];
            float dt = 1f / Mathf.Sqrt(250);
            double eps = rng.NextGaussian();
            float Snew = (float)S0 * Mathf.Exp((float)((mu - 0.5 * vol * vol) * dt + vol * Mathf.Sqrt(dt) * eps));
            prices[comm] = Snew;
        }

    }

    public float GetValue(Commodity commodity)
    {
        return Mathf.RoundToInt(prices[commodity]);
    }
}

public enum Commodity
{
    GOLD, ORE, STEEL, ORICHALCUM
}
