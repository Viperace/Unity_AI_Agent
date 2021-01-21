
public struct Needs
{
	public Needs(float bloodLust, float energy, float shopping, float hp)
	{
		BloodLust = bloodLust;
		Energy = energy;
		Shopping = shopping;
		HP = hp; 
	}

	public float BloodLust { get; set; }
	public float Energy { get; set; }
	public float Shopping { get; set; }
	public float HP { get; set; }

	public override string ToString() => $"({BloodLust}, {Energy}, {Shopping})";

	public static Needs operator + (Needs x, Needs y)
	{
		return new Needs
		{
			BloodLust = x.BloodLust + y.BloodLust,
			Energy = x.Energy + y.Energy,
			Shopping = x.Shopping + y.Shopping,
			HP = x.HP + y.HP
		};
	}
	public static Needs zero { get; }
}

/// <summary>
/// Utility house all needs
/// </summary>
public class Utility
{
	public Utility() { }
	
	public float AggregateScore(Needs needs)
	{
		float bloodUtility = GetYfromRates(needs.BloodLust, GetBloodLustUtilityRate);
		float energytility = GetYfromRates(needs.Energy, GetEnergyUtilityRate);
		float shoppingUtility = GetYfromRates(needs.Shopping, GetShoppingUtilityRate);
		float survivalUtility = GetYfromRates(needs.HP, GetHPUtilityRate);

		float score = bloodUtility + energytility + shoppingUtility + survivalUtility;

		return score;
	}

	float GetBloodLustUtilityRate(float x)
    {
		return 1.5f;
    }

	float GetEnergyUtilityRate(float x)
	{
		if (x < 15)
			return 1.5f;
		else if (x < 70)
			return 1;
		else if (x < 80)
			return 0.5f;
		else if (x < 90)
			return 0.1f;
		else
			return 0f;
	}

	float GetShoppingUtilityRate(float x)
	{
		if (x < 10)
			return 2f;
		else if (x < 70)
			return 1;
		else if (x < 90)
			return 0.5f;
		else
			return 0.1f;
	}

	float GetHPUtilityRate(float x)
	{
		if (x < 10)
			return 100f;
		else if (x < 20)
			return 20f;
		else if (x < 30)
			return 2f;
		else if (x < 40)
			return 1.5f;
		else if (x < 50)
			return 1f;
		else
			return 0f;
	}

	// Give x, return Piecewise linear y
	float GetYfromRates(float x, System.Func<float, float> rateFunc)
	{
		float y = 0;
		float rate = 0;
		for (int i = 0; i < x; i++)
		{
			rate = rateFunc((float)i);
			y += rate * 1f;
		}

		return y;
	}


public float MarginalScore(Needs currentNeeds, Needs expectedNeeds)
	{
		float baseScore = AggregateScore(currentNeeds);
		float newScore = AggregateScore(currentNeeds + expectedNeeds);
		float marginalScore = newScore - baseScore;
		return marginalScore;
	}

}


