using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroCombatStat : CombatStat, ICombatStat
{
    Inventory inventory;
    RolePlayingStatBehavior rpgStatBehavior;

    new void Start()
    {
        inventory = GetComponent<Inventory>();
        rpgStatBehavior = GetComponent<RolePlayingStatBehavior>();
    }

    public new float AttackPower()
    { 
        TallyAttackPower();
        return attackPower; 
    }
    public new float DefendPower()
    {
        TallyDefendPower();
        return defendPower;
    }

    float GetBaseAttackFromLevel()
    {
        int level = rpgStatBehavior.rpgStat.level;
        return level;
    }
    float GetBaseDefendFromLevel()
    {
        int level = rpgStatBehavior.rpgStat.level;
        return level;
    }

    void TallyDefendPower()
    {
        if (CompareTag("Hero") && inventory)
        {
            float baseDefend = 0;
            float defendMod = 0;
            foreach (BasicGear gear in inventory.Equipments)
            {
                baseDefend += gear.defend;
                defendMod += gear.defendBonus;
            }
            float levelDef = GetBaseDefendFromLevel();
            defendPower = levelDef + baseDefend * (1 + defendMod);
        }
        else
            defendPower = 0;
    }
    void TallyAttackPower()
    {
        if (CompareTag("Hero") && inventory)
        {
            // Tally Attack bonuses
            float baseAttack = 0;
            float attackMod = 0;
            foreach (BasicGear gear in inventory.Equipments)
            {
                baseAttack += gear.attack;
                attackMod += gear.attackBonus;
            }
            float levelAtk = GetBaseAttackFromLevel();
            attackPower = levelAtk + baseAttack * (1 + attackMod);
        }
        else
            attackPower = 0;
    }
}
