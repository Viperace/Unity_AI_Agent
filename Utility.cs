public struct Needs
{
    public Needs(float bloodLust, float energy, float shopping, float survival)
    {
        BloodLust = bloodLust;
		Energy = energy;
		Shopping = shopping;
        Survival = survival  // <- to remove. This should be instinct, not plan.
    }

    public float BloodLust { get; set;}
    public float Energy { get; set; }
	public float Shopping { get; set; }
	public float Survival { get; set; }

    public override string ToString() => $"({BloodLust}, {Energy}, {Shopping})";
	
	public static Needs operator +(Needs x, Needs y) 
	{
        return new Needs 
        {
            BloodLust = x.BloodLust + y.BloodLust,
            Energy = x.Energy + y.Energy,
            Shopping = x.Shopping + y.Shopping,
            Survival = x.Survival + y.Survival
        };
	}
}

public class Utility
{
	Needs needs {get; private set;}
	
	public Utility()
	{		
		this.needs = new Needs();
		needs.BloodLust = 80 + Random.Range(-10, 10);
		needs.Shopping = 90 + Random.Range(-10, 10);
		needs.Energy = 90 + Random.Range(-15, 15);
		needs.Survival = 100;
	}
	
	public float AddUtility(Needs needsAddition)
	{
		this.needs += needsAddition;		
	}
		
	public float AggregateScore(Needs needsAddition = new Needs(0,0,0,0))
	{
		// function to combine F(a,b,c) 
	}
	
	public float MarginalScore(Needs needsAddition)
	{
		float baseScore = AggregateScore();
		float newScore = AggregateScore(needsAddition);
		float marginalScore = newScore - baseScore;
		return marginalScore;
	}

}



