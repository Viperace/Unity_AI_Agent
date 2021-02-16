using System.Collections.Generic;
using UnityEngine;

public class Blueprint : BasicGear
{
    public static Dictionary<string, Blueprint> BlueprintsDictionary;
    public static Dictionary<string, List<Blueprint>> SubtypeDictionary;

    public int ore { get; set; }
    public int steel { get; set; }
    public int coal { get; set; }
    public int orichalcum { get; set; }

    public GameObject prefab;
  
	public Blueprint()
    {
        // Create BlueprintsDictionary and SubtypeDictionary
        if (Blueprint.BlueprintsDictionary == null && LoadItem.Instance != null)
        {
            // Create main list
            Blueprint.BlueprintsDictionary = new Dictionary<string, Blueprint>();
            foreach(GearJsonData data in LoadItem.Instance.gearsData.gears)
                BlueprintsDictionary.Add(data.name, GearJsonData.SpawnBlueprint(data));

            // Create by-subtype list
            Blueprint.SubtypeDictionary = new Dictionary<string, List<Blueprint>>();
            foreach(string subtype in GetUniqueSubtypes())
            {
                List<Blueprint> tmpList = new List<Blueprint>();
                foreach (Blueprint b in BlueprintsDictionary.Values)
                    if (b.subtype.Equals(subtype))
                        tmpList.Add(b);
                SubtypeDictionary.Add(subtype, tmpList);
            }
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

    // Copy construct 
    public Blueprint(Blueprint blueprint)
    {
        // Copy values 
        base.name = blueprint.name;
        base.rarity = blueprint.rarity;
        base.attack = blueprint.attack;
        base.defend = blueprint.defend;
        base.attackBonus = blueprint.attackBonus;
        base.defendBonus = blueprint.defendBonus;
        base.durability = blueprint.durability;
        base.prefabName = blueprint.prefabName;
        base.type = blueprint.type;
        base.subtype = blueprint.subtype;
        base.slot = blueprint.slot;
        this.ore = blueprint.ore;
        this.steel = blueprint.steel;
        this.coal = blueprint.coal;
        this.orichalcum = blueprint.orichalcum;
        this.prefab = blueprint.prefab;
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

    public static List<Blueprint> GetBlueprintOfSubtypes(string subtype) => SubtypeDictionary[subtype];

    public static List<Blueprint> GetBlueprintOfSubtypesAndRarity(string subtype, Rarity rarity)
    {
        List<Blueprint> subtypeSet = SubtypeDictionary[subtype];
        List<Blueprint> subtypeAndRaritySet = new List<Blueprint>();
        foreach(Blueprint b in subtypeSet)
            if (b.rarity == rarity && b.subtype == subtype)
                subtypeAndRaritySet.Add(b);
        return subtypeAndRaritySet;
    }

    public static Blueprint SpawnBlueprint(string name)
    {
        foreach(Blueprint b in BlueprintsDictionary.Values)
            if(b.name == name)
                return new Blueprint(b);

        return null;
    }
}
