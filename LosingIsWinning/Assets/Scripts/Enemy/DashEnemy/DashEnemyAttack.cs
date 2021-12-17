using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyAttack : MonoBehaviour
{
    public bool startAttack = false;

    public float m_attackRange;
    [System.NonSerialized] public bool m_movingRight;
    DashEnemyController m_controller;

    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponentInParent<DashEnemyController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
