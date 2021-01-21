using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Combat
{
    Combatant initiator;
    Combatant target;
    CombatStat initiatorStat;
    CombatStat targetStat;


    //CombatResultEnum result;
    CombatResult combatResult;

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

    public CombatResult ComputeResult()
    {
        // Create result
        combatResult = new CombatResult();
        combatResult.Simulate(initiatorStat, targetStat);

        //Debug.Log("Precomputed result: Winner lose HP: " + combatResult.winnerHPlost);

        // Declare winner/loser (runner if there is)
        switch (combatResult.result)
        {
            case CombatResultEnum.InitiatorWin:
                winner = initiator;
                loser = target;
                break;
            case CombatResultEnum.InitiatorLose:
                loser = initiator;
                winner = target;
                break;
            case CombatResultEnum.InitiatorFlee:
                runner = initiator;
                winner = target;
                break;
            case CombatResultEnum.TargetFlee:
                runner = target;
                winner = initiator;
                break;
        }

        return combatResult;
    }

    public CombatResult GetResult() => combatResult;
}

public enum CombatResultEnum
{
    InitiatorWin,
    InitiatorLose,
    InitiatorFlee,
    TargetFlee
}

public class CombatResult
{
    public CombatResultEnum result { get; private set; }
    int winnerHPlost;
    int runnerHPlost;
    int winnerXPgained;
    int winnerFameGained;
    int runnerXPgained;
    int runnerFameGained;
    
    public CombatResult() 
    {
        winnerXPgained = 1;
        winnerFameGained = 1;
        runnerXPgained = 0;
        runnerFameGained = 0;
    }


    // 3 rounds of combat, each combatant has 3 HP. Every combat result in loser -1 hp.
    // By end of 3rd round, whoever has lesser HP lose the combat and killed.
    // Survivor=winner, has actual hp being minused        
    // Probability of attacker win (defender - 1hp) = A /(A + D)
    // TODO: Attacker Flee! 
    public void Simulate(CombatStat attackerStat, CombatStat defenderStat)
    {
        int nrounds = 3;
        int aHP = nrounds;
        int dHP = nrounds;
        float probability_initiator_win = attackerStat.AttackPower / (attackerStat.AttackPower + defenderStat.AttackPower);
        for (int i = 0; i < nrounds; i++)
            if (Random.value < probability_initiator_win)
                dHP--;
            else
                aHP--;

        // Winner HP lost        
        winnerHPlost = nrounds - Mathf.Max(aHP, dHP);

        // runner
        runnerHPlost = winnerHPlost; // todo

        // Result
        if (aHP > dHP)
            result = CombatResultEnum.InitiatorWin;
        else if (dHP > aHP)
            result = CombatResultEnum.InitiatorLose;
        else
            result = CombatResultEnum.InitiatorFlee;

    }

    public KillCreepsEffect GetWinnerEffect()
    {
        KillCreepsEffect winEffect = new KillCreepsEffect();
        winEffect.SetHealthLost(winnerHPlost);
        return winEffect;
    }

    public FleeEffect GetFleeEffect()
    {
        FleeEffect runawayEffect = new FleeEffect();
        runawayEffect.SetHealthLost(runnerHPlost);

        return runawayEffect;
    }
}