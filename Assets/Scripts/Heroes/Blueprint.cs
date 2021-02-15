using System.Collections.Generic;
using UnityEngine;

public class Blueprint : BasicGear
{
    public static Dictionary<string, Blueprint> BlueprintsDictionary;

    public int ore { get; set; }
    public int steel { get; set; }
    public int coal { get; set; }
    public int orichalcum { get; set; }

    public GameObject prefab;
  
	public Blueprint()
    {
        if(Blueprint.BlueprintsDictionary == null && LoadItem.Instance != null)
        {
            Blueprint.BlueprintsDictionary = new Dictionary<string, Blueprint>();
            foreach(GearJsonData data in LoadItem.Instance.gearsData.gears)
                BlueprintsDictionary.Add(data.name, GearJsonData.SpawnBlueprint(data));
        }
    }

    public Blueprint(BasicGear gear, int ore, int steel, int coal, int orichalcum, string prefabFilename)
    {
        // Copy values 
        base.name = gear.name;
        base.rarity = gear.rarity;
        base.attack = gear.attack;
        base.defend = gear.defend;
        base.attackBonus = gear.attackBonus;
        base.defendBonus = gear.defendBonus;
        base.durability = gear.durability;
        base.prefabName = gear.prefabName;
        base.type = gear.type;
        base.subtype = gear.subtype;
        base.slot = gear.slot;

        // New
        this.ore = ore;
        this.steel = steel;
        this.coal = coal;
        this.orichalcum = orichalcum;

        // Init prefab
        if(LoadItem.Instance)
            prefab = LoadItem.Instance.GetPrefabByName(prefabFilename);
    }

    public static string[] GetUniqueSubtypes()
    {
        List<string> uniqueSubtypes = new List<string>();
        foreach(Blueprint b in BlueprintsDictionary.Values)
        {
            if (!uniqueSubtypes.Contains(b.subtype))
                uniqueSubtypes.Add(b.subtype);
        }

        return uniqueSubtypes.ToArray();
    }

    public static List<Blueprint> GetBlueprintOfSubtypes(string subtype)
    {
        List<Blueprint> belongedSubtypes = new List<Blueprint>();
        foreach (Blueprint b in BlueprintsDictionary.Values)
        {
            if (b.subtype.Equals(subtype, System.StringComparison.CurrentCultureIgnoreCase))
                belongedSubtypes.Add(b);
        }
        return belongedSubtypes;
    }
}
