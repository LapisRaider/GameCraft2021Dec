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

    protected int maxHealth = 10;
    protected int health;
    protected Vector3 m_nextPosition;
    protected Rigidbody2D m_rb;


    float hitFlashTimer_;

    virtual public void Awake()
    {
        health = maxHealth;
        Init();
    }

    virtual public void Init()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {
//#if UNITY_EDITOR
//        Debug.DrawLine(transform.position, DEBUG_TARGET.position, m_triggered ? line_triggered : line_normal);
//#endif
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
