using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] int coins;
    [SerializeField] List<BasicGear> equipments;
    void Start()
    {
        coins = 0;
        equipments = BasicGear.GenerateBasicEquipments();
    }

    public void Add(BasicGear gear)
    {
        equipments.Remove(null);
        equipments.Add(gear);
    }
    public void AddCoins(int number) => coins += number;
    public void RemoveCoins(int number) => coins -= number;
    public string SummaryString()
    {
        string str = string.Concat("Coin:", coins, ". Gear:");
        for (int i = 0; i < equipments.Count; i++)
        {
            string symbol = i == equipments.Count - 1 ? "." : ",";
            str = string.Concat(str, equipments[i].name, symbol);
        }
        return str;
    }    
    public void UpdateGearDurability(float changes) // Pick all gear and reduce its durability
    { 
        foreach (BasicGear gear in equipments)
        {
            gear.durability += changes;
            gear.durability = Mathf.Clamp01(gear.durability);
        }
    }

    #region Getters
    public List<BasicGear> Equipments { get { return equipments; } }

    float averageDurability;
    public float AverageDurability
    { 
        get 
        {
            averageDurability = 0;
            foreach(BasicGear gear in equipments)
                averageDurability += gear.durability;
            averageDurability /= equipments.Count;
            return averageDurability;
        } 
    }

    float minDurability;
    public float MinimumDurability
    {
        get
        {
            minDurability = 0;
            foreach (BasicGear gear in equipments)
                minDurability = Mathf.Min(gear.durability, minDurability);            
            return minDurability;
        }
    }
    #endregion
}

