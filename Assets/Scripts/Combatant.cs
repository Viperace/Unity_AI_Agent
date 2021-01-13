// FIXME: If notification failure, still will continue to chase target
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CombatStat))]
public class Combatant : MonoBehaviour
{

    [SerializeField] float _attackRange;

    [SerializeField] bool IsFighting = false;

    [SerializeField] float notifyDistance = 5; // Distance needed for target to response

    Combat combat = null;

    Actor actor;

    NavMeshAgent navMeshAgent;

    CombatStat combatStat;

    SlowLookAt slowLook;

    float _cooldown1 = 0;
    float _cooldown2 = 1;

    void Start()
    {
        actor = GetComponent<Actor>();

        combatStat = GetComponent<CombatStat>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        slowLook = GetComponent<SlowLookAt>();

        if (_attackRange > notifyDistance)
            throw new System.Exception("notify Distance > weapon Range");

    }

    public void ChaseAndAttack(Combatant target, System.Action notifyFailureCallback = null)
    {
        Combatant combatant = this.GetComponent<Combatant>();
        if (combatant)
        {
            System.Action attackNoticeCallback = () =>
            {
                if (NotifyCombat(target))
                {
                    Debug.Log("Attack msg sent. Start killing!");
                    _BeginAttackSequence(target);
                }
                else
                {
                    if (notifyFailureCallback != null)
                        notifyFailureCallback();
                }
                                   
            };

            IAgentAction chaseAction = new GotoTarget(this.gameObject, target.gameObject, notifyDistance * 0.8f, attackNoticeCallback);
            actor.SetCurrentAction(chaseAction);
            chaseAction.Run();
        }
        else
            Debug.Log(this.name + ".This actor cannot attack.");
    }

    bool NotifyCombat(Combatant target)
    {
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        if (target.IsFighting)
        {
            Debug.Log(target + " is fighting. Not free.");
            return false;
        }
        else if(dist > notifyDistance)
        {
            Debug.Log(target + " is too far to acknolwedge combat." + dist);
            return false;
        }
        else
        {
            // Combat Calculator
            combat = new Combat(this, target, this.combatStat, target.combatStat);
            combat.ComputeResult();

            // Send result
            target.ReceiveCombatNotification(this, combat);
            this.IsFighting = true;

            return true;
        }        
    }

    void  _BeginAttackSequence(Combatant target)
    {
        // line up, do attk
        AttackSequenceCoroutine = AttackSequence(target.transform);
        StartCoroutine(AttackSequenceCoroutine);
    }

    public void ReceiveCombatNotification(Combatant initiator, Combat combat)
    {
        // Stop current task
        actor.SetCurrentAction(null);

        // Set combat reference
        this.combat = combat;

        // Begin attack
        this.IsFighting = true;
        _BeginAttackSequence(initiator);
    }

    IEnumerator AttackSequenceCoroutine;

    IEnumerator AttackAnimationCoroutine;

    IEnumerator AttackSequence(Transform target)
    {

        // Initialize Face toward
        slowLook.InitiateLookAt(target);

        // Close in
        while (Vector3.Distance(this.transform.position, target.position) > AttackRange )
        {
            // Move toward
            navMeshAgent.SetDestination(target.position);
            
            yield return new WaitForSeconds(0.1f);
        }
        //navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
        //navMeshAgent.transform.LookAt(target);
        slowLook.InitiateLookAt(target);
        Debug.Log("Reach attack range");

        // Begin attack
        // TODO: duration updat
        AttackAnimationCoroutine = AttackAnimation(Time.time + 3);
        StartCoroutine(AttackAnimationCoroutine);
        
    }


    IEnumerator AttackAnimation(float endTime)
    {
        // Do attack (subject to cooldown), unttil time run out. Special atk prioritize
        while (Time.time < endTime)
        {
            //if (combatStat.attack2Cooldown < 0 && anim.IsPlayingIdle())
            if (combatStat.attack2Cooldown < 0)
            {
                //anim.AnimateSpecialAttack();
                _cooldown2 = combatStat.attack2Cooldown;
            }
            //else if (combatStat.attack1Cooldown < 0 && anim.IsPlayingIdle())
            else if (combatStat.attack1Cooldown < 0)
            {
                //anim.AnimateNormalAttack();
                _cooldown1 = combatStat.attack1Cooldown;
            }

            _cooldown1 -= Time.deltaTime;
            _cooldown2 -= Time.deltaTime;
            yield return null;
        }

        DoneCombat();
    }

    // Stuff to set after finish combat
    void DoneCombat()
    {
        // Set off
        IsFighting = false;

        // Do stuff based on result
        if (this == combat.winner)
            Debug.Log(this + " do victory lap");
        else if (this == combat.loser)
            Debug.Log(this + " do death");
        else if (this == combat.runner)
            Debug.Log(this + " do running away from " + combat.winner);

        // Reset 
        combat = null;
    }

    public float AttackRange { get { return _attackRange; } }
}

