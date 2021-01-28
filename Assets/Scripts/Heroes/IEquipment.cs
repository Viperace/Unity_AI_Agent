using System.Collections.Generic;

public interface IEquipment
{
    
}

[System.Serializable]
public class BasicGear : IEquipment
{
    public string name;
    public Rarity rarity;
    public EquipmentSlot slot;
    public int attack;
    public int defend;
    public float attackBonus; // Percentage. default = 0%
    public float defendBonus;
    public float durability;  // 0 to 1
    public BasicGear() 
    {
        name = "";
        rarity = Rarity.COMMON;
        slot = EquipmentSlot.BODY;
        attack = 1;
        defend = 0;
        attackBonus = 0;
        defendBonus = 0;
        durability = 1f;
    }
    public BasicGear(string name, Rarity rarity, EquipmentSlot slot, int attack, int defend, 
        float attackBonus = 0, float defendBonus = 0, float durability = 1f)
    {
        this.name = name;
        this.rarity = rarity;
        this.slot = slot;
        this.attack = attack;
        this.defend = defend;
        this.attackBonus = attackBonus;
        this.defendBonus = defendBonus;
        this.durability = durability;
    }
    public static List<BasicGear> GenerateBasicEquipments()
    {        
        List<BasicGear> gears = new List<BasicGear>();
        //gears.Add(new BasicGear("Leather Armor", Rarity.COMMON, EquipmentSlot.BODY, 0, 1));
        //gears.Add(new BasicGear("Wooden Stick", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 1, 0));
        // gears.Add(LoadItem.Instance.Spawn("Crossbow"));
        gears.Add(LoadItem.Instance.SpawnRandomItem());
        gears.Add(LoadItem.Instance.SpawnRandomItem());
        return gears;
    }
}

public enum Rarity
{
    COMMON = 0,
    LEGENDARY
}
public enum EquipmentSlot
{
    LEFT_HAND,
    RIGHT_HAND,
    BOTH_HAND,
    HEAD,
    ARM,
    BODY,
    LEG
}