using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Combat
{
    Combatant initiator;
    Combatant target;
    CombatStat initiatorStat;
    CombatStat targetStat;

    CombatResult result;

    public Combatant winner {get; private set; }
    public Combatant loser {get; private set; }
    public Combatant runner {get; private set; }

    public Combat() { }

    public Combat(Combatant initiator, Combatant target, CombatStat initiatorStat, CombatStat targetStat)
    {
        this.initiator = initiator;
        this.target = target;
        this.initiatorStat = initiatorStat;
        this.targetStat = targetStat;
        winner = loser = runner = null;
    }

    public void ComputeResult()
    {
        // Take in stat
        if (Random.value > 0.5f)
            result = CombatResult.InitiatorWin;
        else
            result = CombatResult.InitiatorLose;
        
        Debug.Log(initiator + " " + result);

        switch (result)
        {
            case CombatResult.InitiatorWin:
                winner = initiator;
                loser = target;
                break;
            case CombatResult.InitiatorLose:
                loser = initiator;
                winner = target;
                break;
            case CombatResult.InitiatorFlee:
                runner = initiator;
                winner = target;
                break;
            case CombatResult.TargetFlee:
                runner = target;
                winner = initiator;
                break;
        }
    }

}

public enum CombatResult
{
    InitiatorWin,
    InitiatorLose,
    InitiatorFlee,
    TargetFlee
}