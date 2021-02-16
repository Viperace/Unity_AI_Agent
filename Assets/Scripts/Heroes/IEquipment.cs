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
    public string type;
    public string subtype;
    public int attack;
    public int defend;
    public float attackBonus; // Percentage. default = 0%
    public float defendBonus;
    public float durability;  // 0 to 1
    public string prefabName;
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
        prefabName = "";
    }
    public BasicGear(string name, Rarity rarity, EquipmentSlot slot, int attack, int defend, 
        float attackBonus = 0, float defendBonus = 0, float durability = 1f, string prefabName="")
    {
        this.name = name;
        this.rarity = rarity;
        this.slot = slot;
        this.attack = attack;
        this.defend = defend;
        this.attackBonus = attackBonus;
        this.defendBonus = defendBonus;
        this.durability = durability;
        this.prefabName = prefabName;
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
    
    public static BasicGear CreateFromBlueprint(Blueprint blueprint)
    {
        BasicGear gear = new BasicGear(blueprint.name, 
            blueprint.rarity, blueprint.slot, blueprint.attack, blueprint.defend, 
            blueprint.attackBonus, blueprint.defendBonus, blueprint.durability, blueprint.prefabName);		
        return gear;
    }
}

public enum Rarity
{
    CRUDE = 0,
    COMMON = 1,
    FINE = 2,
    EXCEPTIONAL = 3,
    LEGENDARY= 4 
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
