using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public enum EnemyAnimations
{
    MeleePlant,
    MeleeIdle,
    MeleeAttack,
    MeleeRun,
    MeleeDeath,
    MeleeHit
}

public class Melee_PlantState : State
{
    GameObject go_;
    public Melee_PlantState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }
    public override void Enter()
    {
        Debug.Log("Testing Plant State");
        go_.GetComponent<MobMelee>().m_Animator.Play(Enum.GetName(typeof(EnemyAnimations), EnemyAnimations.MeleePlant));
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Testing plant state");
    }
}
public class Melee_HitState : State
{
    GameObject go_;
    public Melee_HitState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }
}
public class Melee_IdleState : State
{
    void CheckDead()
    {
        
    }

    GameObject go_;     // The attached game object
    RaycastHit hit_;    // To detect hits

    public Melee_IdleState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }

    // Enter is called before the first frame update
    public override void Enter()
    {
        Debug.Log("Testing Idle state");
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!go_.activeSelf)
            return;

        Debug.Log("Testing Idle state");
        go_.GetComponent<MobMelee>().m_Animator.Play(Enum.GetName(typeof(EnemyAnimations), EnemyAnimations.MeleeIdle));


        CheckDead();
    }

    public override void Exit()
    {
    
    }
}

public class Melee_RunState : State
{
    void CheckDead()
    {

    }

    GameObject go_;     // The attached game object
    RaycastHit hit_;    // To detect hits

    public Melee_RunState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }

    // Enter is called before the first frame update
    public override void Enter()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!go_.activeSelf)
            return;



        CheckDead();
    }

    public override void Exit()
    {

    }
}

public class Melee_AttackState : State
{
    void CheckDead()
    {

    }

    GameObject go_;     // The attached game object
    RaycastHit hit_;    // To detect hits

    public Melee_AttackState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }

    // Enter is called before the first frame update
    public override void Enter()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!go_.activeSelf)
            return;



        CheckDead();
    }

    public override void Exit()
    {

    }
}

public class Melee_DeadState : State
{
    void CheckDead()
    {

    }

    GameObject go_;     // The attached game object
    RaycastHit hit_;    // To detect hits

    public Melee_DeadState(string m_stateID, GameObject _go) : base(m_stateID) { go_ = _go; }

    // Enter is called before the first frame update
    public override void Enter()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!go_.activeSelf)
            return;



        CheckDead();
    }

    public override void Exit()
    {

    }
}