public interface IUtilityEffect
{
    public void Apply(NeedsBehavior needsBehavior);
}

public interface IRolePlayingEffect
{
    public void Apply(RolePlayingStat rpgStat);
}
public interface IInventoryEffect
{
    public void Apply(Inventory inventory);
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

public class KillCreepsEffect : IUtilityEffect, IRolePlayingEffect, IInventoryEffect
{
    float healthLost;
    float xpGained;
    public KillCreepsEffect() {}
    public void SetHealthLost(float healthLost) => this.healthLost = healthLost;
    public void SetExperiencePointGain(float xpGained) => this.xpGained = xpGained;

    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(30, -10, -5, -healthLost);
        needsBehavior.AddNeeds(x);
    }
    public void Apply(RolePlayingStat rpgStat)
    {
        rpgStat.experiencePoint += xpGained;
    }
    public void Apply(Inventory inventory)
    {
        if(inventory)
            inventory.UpdateGearDurability(-0.1f);
    }
    public void ApplyAll(NeedsBehavior needsBehavior, RolePlayingStat rpgStat, Inventory inventory)
    {
        Apply(needsBehavior);
        Apply(rpgStat);
        Apply(inventory);
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


public class ShoppingEffect : IUtilityEffect, IInventoryEffect
{
    public ShoppingEffect() { }
    public void Apply(NeedsBehavior needsBehavior)
    {
        Needs x = new Needs(0, 0, 65, 0);
        needsBehavior.AddNeeds(x);
    }

    public void Apply(Inventory inventory)
    {
        inventory.UpdateGearDurability(1f);
    }
}

