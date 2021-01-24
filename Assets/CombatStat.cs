using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStat : MonoBehaviour, ICombatStat
{
    [SerializeField] protected float attackPower;
    [SerializeField] protected float defendPower;
    public float attack1Cooldown;
    public float attack2Cooldown;

    protected void Start()
    {
    }
    public float AttackPower()
    {
        return attackPower;
    }
    public float DefendPower()
    {
        return defendPower;
    }
}

public interface ICombatStat
{
    public float AttackPower();
    public float DefendPower();
}