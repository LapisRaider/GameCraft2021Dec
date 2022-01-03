using System.Collections;
using System;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{

    [Header("Movement Variables")]
    public float m_speed;
    [Tooltip("For raycasting")]
    public float m_distance;
    [Tooltip("How long they patrol and idle for")]
    public float m_patrolTime;
    [Header("Player Detection")]
    public float m_detectionRange;
    public float m_attackRange;

    [System.NonSerialized] public bool m_movingRight = true;
    bool m_moving;
    bool m_attacking;
    float m_patrolTimer;
    Animator m_Animator;

    // Start is called before the first frame update
    override public void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
        m_currState = m_targetState = EnemyStates.MeleePlant;
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }
    /// <summary>
    /// When the enemy changes state, this function SHOULD run once i hope
    /// 
    /// </summary>
    /// <param name="targetState">State that it is changing to</param>
    protected override void ChangeState(EnemyStates targetState)
    {
        base.ChangeState(targetState);
        switch (targetState)
        {
            case EnemyStates.MeleePlant:
                {
                    m_Animator.Play(Enum.GetName(typeof(EnemyAnimations), EnemyAnimations.MeleePlant));

                }
                break;
            case EnemyStates.MeleeIdle:
                {
                    // Can play the smoke poof to changing to a plant and all here
                    m_Animator.Play(Enum.GetName(typeof(EnemyAnimations), EnemyAnimations.MeleeIdle));
                }
                break;
            case EnemyStates.MeleeRunning:
                {

                }
                break;
            case EnemyStates.MeleeAttack:
                {

                }
                break;
            case EnemyStates.MeleeDeath:
                {

                }
                break;
        }
    }

    protected override void UpdateState(EnemyStates currState)
    {
        base.UpdateState(currState);
        switch (currState)
        {
            case EnemyStates.MeleePlant:
                {
                    if(Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        if (m_isMorphed == false)
                        {
                            m_targetState = EnemyStates.MeleeIdle;
                            m_isMorphed = true;
                        }
                    }
                }
                break;
            case EnemyStates.MeleeIdle:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        if (m_isMorphed == true)
                        {
                            m_targetState = EnemyStates.MeleePlant;
                            m_isMorphed = false;
                        }

                    }
                }
                break;
            case EnemyStates.MeleeRunning:
                {

                }
                break;
            case EnemyStates.MeleeAttack:
                {

                }
                break;
            case EnemyStates.MeleeDeath:
                {

                }
                break;
        }
    }

    override public bool TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if (health > 0)
        {

        }
        return true;
    }


    override protected void OnDeath()
    {
        base.OnDeath();
    }

    public enum EnemyAnimations
    {
        MeleePlant,
        MeleeIdle,
        MeleeAttack,
        MeleeRun,
        MeleeDeath,
        MeleeHit
    }
}
