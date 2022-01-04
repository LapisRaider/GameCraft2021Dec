using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMelee : MobBase
{
    [System.NonSerialized]
    public StateMachine sm;

    [Tooltip("For raycasting")]
    public float m_RayDistance;
    [Tooltip("How long they patrol and idle for")]
    public float m_patrolTime;

    [System.NonSerialized] public bool m_movingRight = true;
    bool m_moving;
    bool m_attacking;
    float m_patrolTimer;
    [System.NonSerialized] public Animator m_Animator;

    public override void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
        sm = new StateMachine();
        sm.AddState(new Melee_PlantState("MeleePlantState", this.gameObject));
        sm.AddState(new Melee_IdleState("MeleeIdleState", this.gameObject));
        sm.AddState(new Melee_RunState("MeleeRunState", this.gameObject));
        sm.AddState(new Melee_AttackState("MeleeAttackState", this.gameObject));
        sm.AddState(new Melee_DeadState("MeleeDeadState", this.gameObject));
        sm.AddState(new Melee_HitState("MeleeHitState", this.gameObject));
        Debug.Log("Hello");
        sm.SetNextState("MeleePlantState");

    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            {
                if (m_isMorphed == false)
                {
                    sm.SetNextState("MeleeIdleState");
                    m_isMorphed = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (m_isMorphed == true)
            {
                sm.SetNextState("MeleePlantState");
                m_isMorphed = false;
            }
        }

        if (sm != null)
        {
            sm.Update();
        }
    }
}
