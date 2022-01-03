using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    #if UNITY_EDITOR
    public static bool g_debugMode = true;
    static Color32 line_normal = new Color32(0, 0, 255, 255);
    static Color32 line_triggered = new Color32(255, 0, 0, 255);
    #endif

    protected bool m_triggered = false;

    
    protected int health;
    protected Vector3 m_nextPosition;
    protected Rigidbody2D m_rb;

    protected bool m_isMorphed = false;

    public enum EnemyStates
    {
        MeleePlant,
        MeleeIdle,
        MeleeAttack,
        MeleeRunning,
        MeleeDeath
    }

    [System.NonSerialized] public EnemyStates m_currState;
    [System.NonSerialized] public EnemyStates m_targetState;

    float hitFlashTimer_;

    virtual public void Awake()
    {
       // health = maxHealth;
        Init();
    }

    virtual public void Init()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {
        // Changing enemy state
        if (m_currState != m_targetState)
        {
            ChangeState(m_targetState);

            m_currState = m_targetState;
        }

        UpdateState(m_currState);
    }

    virtual protected void ChangeState(EnemyStates targetState)
    {

    }

    virtual protected void UpdateState(EnemyStates currState)
    {

    }

    virtual public bool TakeDamage(int dmg)
    {
        health -= dmg;
   
        if (health <= 0) 
        {
            return true;
        }

        return false;
    }

    virtual protected void OnDeath()
    {

    }

    /// <summary>
    /// Sets the gameobject inactive
    /// </summary>
    virtual public void Remove()
    {
        gameObject.SetActive(false);
    }
}
