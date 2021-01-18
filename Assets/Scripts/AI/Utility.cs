using UnityEngine;

public struct Needs
{
	public Needs(float bloodLust, float energy, float shopping, float hp)
	{
		BloodLust = bloodLust;
		Energy = energy;
		Shopping = shopping;
		HP = hp;  // <- to remove. This should be instinct, not plan.
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
	public Utility()
	{

	}

	public float AggregateScore(Needs needs)
	{
		float bloodUtility = GetBloodLustUtilityRate(needs.BloodLust) * needs.BloodLust;
		float energytility = GetEnergyUtilityRate(needs.Energy) * needs.Energy;
		float shoppingUtility = GetShoppingUtilityRate(needs.Shopping) * needs.Shopping;
		float survivalUtility = GetHPUtilityRate(needs.HP) * needs.HP;

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

	public float MarginalScore(Needs currentNeeds, Needs expectedNeeds)
	{
		float baseScore = AggregateScore(currentNeeds);
		float newScore = AggregateScore(expectedNeeds);
		float marginalScore = newScore - baseScore;
		return marginalScore;
	}

}



