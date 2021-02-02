using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Superbest_random;

public class Market : MonoBehaviour
{
    static Market _instance;
    public static Market Instance { get { return _instance; } }

    Dictionary<Commodity, float> prices = new Dictionary<Commodity, float>();
	Dictionary<Commodity, float> volatilities = new Dictionary<Commodity, float>();
    float _stepSize = 60; // in seconds
    float _stepCooldown;
	
	
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
		
		// Init vol
		volatilities.Add(Commodity.GOLD, 0);
        volatilities.Add(Commodity.ORE, 0.3);
        volatilities.Add(Commodity.STEEL, 0.15);
        volatilities.Add(Commodity.ORICHALCUM, 0.2);		
    }

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
		/* dS = mu*dt + sig*dW
			S(t)=S(t-1)*exp((mu-0.5*sig^2)*T + sig*sqrt(T)*Z)
			where Z~N(0, 1)
		*/
		
		// TODO: Move to start()?
		var rng = new Random();
		foreach(Commodity comm in Enum.GetValues(typeof(Commodity)))
		{
			//Commodity comm = Commodity.ORE;
			float vol = volatilities[comm];
			float S0 = prices[comm];
			float dt = _stepSize / Mathf.Sqrt(250);
			float eps = rng.NextGaussian();
			float Snew = S0*Mathf.Exp(-0.5*vol*vol*dt + vol*Mathf.Sqrt(dt)*eps);
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
