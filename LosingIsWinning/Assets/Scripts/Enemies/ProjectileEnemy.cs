using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : EnemyBase
{
    [Header("Player Detection")]
    public float m_detectionRange;
    public float m_attackRange;

    [System.NonSerialized] public bool m_movingRight = true;
    bool m_attacking;
    float m_patrolTimer;

    // Start is called before the first frame update
    override public void Awake()
    {
        base.Awake();

    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }

    /// <summary>
    /// When the enemy changes
    /// </summary>
    /// <param name="targetState"></param>
    protected override void ChangeState(EnemyStates targetState)
    {
        base.ChangeState(targetState);
        
    }

    protected override void UpdateState(EnemyStates currState)
    {
        base.UpdateState(currState);

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
        MeleeIdle,
        MeleeAttack,
        MeleeRun,
        MeleeDeath,
        MeleeHit
    }
}
