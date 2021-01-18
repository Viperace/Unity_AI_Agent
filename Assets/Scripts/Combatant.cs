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

    System.Action onWinAction = null;
    System.Action onLoseAction = null;
    //System.Action onFleeAction;
    System.Action onComplete = null;
    System.Action onFailAction = null;

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

    public void ChaseAndAttack(Combatant target, System.Action onFailCallback = null,
         System.Action onCombatComplete = null)
    {
        //Init
        this.onComplete = onCombatComplete;
        this.onFailAction = onFailCallback;
        Combatant combatant = this.GetComponent<Combatant>();
        Combatant targetCombatant = target.GetComponent<Combatant>();


        if (combatant && combatant.enabled && targetCombatant)
        {
            System.Action attackNoticeCallback = () =>
            {
                if (NotifyCombat(target))
                {
                    Debug.Log("Attack msg sent. Start killing!");
                    _BeginAttackSequence(target);
                }
                else
                    OnFailure();                                   
            };

            IAgentAction chaseAction = new GotoTarget(this.gameObject, target.gameObject, notifyDistance * 0.8f, attackNoticeCallback);
            actor.SetCurrentAction(chaseAction);
            chaseAction.Run();
        }
        else
        {
            Debug.Log(this.name + ".This actor/target cannot attack.");
            OnFailure();
        }            
    }

    bool NotifyCombat(Combatant target)
    {
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        if (!target.enabled)
        {
            Debug.Log("Target combatant not enabled");
            return false;
        }
        else if (target.IsFighting)
        {
            Debug.Log(target + " is fighting. Not free.");
            return false;
        }
        else if (dist > notifyDistance)
        {
            Debug.Log(target + " is too far to acknolwedge combat." + dist);
            return false;
        }
        else
        {
            // Combat Calculator
            combat = new Combat(this, target, this.combatStat, target.combatStat);
            combat.ComputeResult();
            CombatResult result = combat.GetResult();

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

        slowLook.StopLooking();

        // Do stuff based on result
        if (this == combat.winner)
        {
            OnWinAction();
            Debug.Log(this.gameObject + " do victory lap");
        }            
        else if (this == combat.loser)
        {
            OnLoseAction();
            Debug.Log(this.gameObject + " do death");
        }            
        else if (this == combat.runner)
        {
            OnFleeAction();
            Debug.Log(this.gameObject + " do running away from " + combat.winner);
        }

        OnComplete();        
    }

    void OnWinAction()
    {
        if (onWinAction != null)
            onWinAction();
    }

    void OnLoseAction()
    {
        actor.DoDeath();

        if (onLoseAction != null)
            onLoseAction();
    }

    void OnFleeAction()
    {
        throw new System.Exception("Not yet implemented");
    }

    void OnComplete()
    {
        // Reset 
        combat = null;
        actor.SetCurrentAction(null);

        if (onComplete != null)
            onComplete();        
    }

    void OnFailure()
    {
        // Reset 
        combat = null;
        actor.SetCurrentAction(null);

        if (onFailAction != null)
            onFailAction();

        Debug.Log("Attack fail");
    }

    public void AddOnWinAction(System.Action act) => onWinAction += act;

    public void AddOnLoseAction(System.Action act) => onLoseAction += act;

    public void AddOnFailAction(System.Action act) => onFailAction += act;

    public float AttackRange { get { return _attackRange; } }
}

