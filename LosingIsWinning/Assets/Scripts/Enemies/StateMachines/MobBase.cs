using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBase : MonoBehaviour
{
    [Header("AI Configuration")]
    public int m_maxHealth;
    public int m_Health;
    public float m_Speed;
    protected bool m_isMorphed = false;

    [Header("Player Detection")]
    public float m_detectionRange;
    public float m_attackRange;

    virtual public void Awake()
    {

    }

    // Start is called before the first frame update
    virtual public void Start()
    {
        
    }

    // Update is called once per frame
    virtual public void Update()
    {
        
    }

    virtual public bool TakeDamage(int dmg)
    {
        m_Health -= dmg;

        if (m_Health <= 0)
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
