using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Combat
{
    Combatant initiator;
    Combatant target;
    ICombatStat initiatorStat;
    ICombatStat targetStat;


    //CombatResultEnum result;
    CombatResult combatResult;

    public Combatant winner {get; private set; }
    public Combatant loser {get; private set; }
    public Combatant runner {get; private set; }

    public Combat() { }

    public Combat(Combatant initiator, Combatant target, ICombatStat initiatorStat, ICombatStat targetStat)
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
    
    int totalCombatRounds = 3;  // Total round  to roll dice
    int winnerRoundLost; 
    int runnerRoundLost;
    float roundLostToHPmodifier = 10f; 
    int winnerXPgained;
    int winnerFameGained;
    int runnerXPgained;
    int runnerFameGained;
    
    public CombatResult() 
    {
        winnerFameGained = 1;
        runnerXPgained = 0;
        runnerFameGained = 0;
    }

    int CalculateWinnerExperienceGained(Actor target)
    {
        // Find out target level
        int targetLevel;
        if (target && target.GetComponent<RolePlayingStatBehavior>()) // Is Hero ?
        {
            targetLevel = target.GetComponent<RolePlayingStatBehavior>().Level;            
        } 
        else if (target && target.GetComponent<Creeps>()) // Is Creeps
        {
            targetLevel = target.GetComponent<Creeps>().Level;
        }
        else
            return 1;

        // Decide threshold
        if (targetLevel == 1)
            return 1;
        else if(targetLevel == 2)
            return 2;
        else if (targetLevel <= 5 )
            return Mathf.RoundToInt(targetLevel * 1.5f);
        else if (targetLevel <= 7)
            return Mathf.RoundToInt(targetLevel * 2f);
        else if (targetLevel <= 9)
            return Mathf.RoundToInt(targetLevel * 3f);
        else
            return Mathf.RoundToInt(targetLevel * 4f);
    }

    // 3 rounds of combat, each combatant has 3 HP. Every combat result in loser -1 hp.
    // By end of 3rd round, whoever has lesser HP lose the combat and killed.
    // Survivor=winner, has actual hp being minused        
    // Probability of attacker win (defender - 1hp) = A /(A + D)
    // TODO: Attacker Flee! 
    public void Simulate(ICombatStat attackerStat, ICombatStat defenderStat)
    {        
        int aHP = totalCombatRounds;
        int dHP = totalCombatRounds;
        float probability_initiator_win = attackerStat.AttackPower() / (attackerStat.AttackPower() + defenderStat.AttackPower());
        for (int i = 0; i < totalCombatRounds; i++)
            if (Random.value < probability_initiator_win)
                dHP--;
            else
                aHP--;

        // Winner HP lost        
        winnerRoundLost = totalCombatRounds - Mathf.Max(aHP, dHP);

        // runner
        runnerRoundLost = winnerRoundLost; // todo

        // Result
        if (aHP > dHP)
            result = CombatResultEnum.InitiatorWin;
        else if (dHP > aHP)
            result = CombatResultEnum.InitiatorLose;
        else
            result = CombatResultEnum.InitiatorFlee;

    }

    public KillCreepsEffect GetWinnerEffect(Actor target)
    {
        KillCreepsEffect winEffect = new KillCreepsEffect();
        
        winEffect.SetHealthLost(winnerRoundLost * roundLostToHPmodifier);

        winnerXPgained = CalculateWinnerExperienceGained(target);
        winEffect.SetExperiencePointGain(winnerXPgained);

        return winEffect;
    }

    public FleeEffect GetFleeEffect()
    {
        FleeEffect runawayEffect = new FleeEffect();
        runawayEffect.SetHealthLost(runnerRoundLost * roundLostToHPmodifier);

        return runawayEffect;
    }
}