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
    public int basePower;
    public float durability;  // 0 to 1
    public BasicGear() 
    {
        name = "";
        rarity = Rarity.COMMON;
        slot = EquipmentSlot.BODY;
        basePower = 1;
        durability = 1f;
    }
    public BasicGear(string name, Rarity rarity, EquipmentSlot slot, int power, float durability = 1f)
    {
        this.name = name;
        this.rarity = rarity;
        this.slot = slot;
        this.basePower = power;
        this.durability = durability;
    }
    public static List<BasicGear> GenerateBasicEquipments()
    {        
        List<BasicGear> gears = new List<BasicGear>();
        gears.Add(new BasicGear("Leather Armor", Rarity.COMMON, EquipmentSlot.BODY, 1));
        gears.Add(new BasicGear("Wooden Stick", Rarity.COMMON, EquipmentSlot.MAIN_WEAPON, 1));
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
    MAIN_WEAPON,
    AUX_WEAPON,
    ARM,
    BODY,
    LEG
}