public interface IUtilityEffect
{
    public void Apply(NeedsBehavior needsBehavior);
}

public class RestAtTownEffect : IUtilityEffect
{
    public RestAtTownEffect() {}
    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(0, 100, 10, 30);
        needsBehavior.AddNeeds(x);
    }
}

public class KillCreepsEffect : IUtilityEffect
{
    float healthLost;
    public KillCreepsEffect() {}
    public void SetHealthLost(float healthLost) => this.healthLost = healthLost;
    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(30, -10, 0, -healthLost);
        needsBehavior.AddNeeds(x);
    }
}

public class FleeEffect : IUtilityEffect
{
    float healthLost;
    public FleeEffect() { }
    public void SetHealthLost(float healthLost) => this.healthLost = healthLost;
    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(0, -10, 0, -healthLost);
        needsBehavior.AddNeeds(x);
    }
}


public class ShoppingEffect : IUtilityEffect
{
    public ShoppingEffect() { }
    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(0, 0, 35, 0);
        needsBehavior.AddNeeds(x);
    }
}

