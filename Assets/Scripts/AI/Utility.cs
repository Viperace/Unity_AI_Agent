using UnityEngine;

public struct Needs
{
	public Needs(float bloodLust, float energy, float shopping, float survival)
	{
		BloodLust = bloodLust;
		Energy = energy;
		Shopping = shopping;
		Survival = survival;  // <- to remove. This should be instinct, not plan.
	}

	public float BloodLust { get; set; }
	public float Energy { get; set; }
	public float Shopping { get; set; }
	public float Survival { get; set; }

	public override string ToString() => $"({BloodLust}, {Energy}, {Shopping})";

	public static Needs operator + (Needs x, Needs y)
	{
		return new Needs
		{
			BloodLust = x.BloodLust + y.BloodLust,
			Energy = x.Energy + y.Energy,
			Shopping = x.Shopping + y.Shopping,
			Survival = x.Survival + y.Survival
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
		// TODO: function to combine F(a,b,c) 
		float score = 0;
		score = needs.BloodLust + needs.Energy + needs.Shopping + needs.Survival;

		return score;
	}


	public float MarginalScore(Needs currentNeeds, Needs expectedNeeds)
	{
		float baseScore = AggregateScore(currentNeeds);
		float newScore = AggregateScore(expectedNeeds);
		float marginalScore = newScore - baseScore;
		return marginalScore;
	}

}



