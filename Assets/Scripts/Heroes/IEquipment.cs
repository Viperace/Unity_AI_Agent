using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    
}

public class BasicGear : IEquipment
{
    public string name { get; set; }
    public Rarity rarity { get; set; }
    public EquipmentSlot slot { get; set; }
    public int basePower { get; set; }
    public BasicGear() 
    {
        name = "";
        rarity = Rarity.COMMON;
        slot = EquipmentSlot.BODY;
        basePower = 1;
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