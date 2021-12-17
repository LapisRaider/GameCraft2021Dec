using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyHit : HitObjs
{
    DashEnemyController m_controller;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_controller = GetComponentInParent<DashEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override bool Hit()
    {
        if (m_HasBeenHit)
            return false;

        if (m_spriteRenderer == null)
            return false;

        m_HasBeenHit = true;
        m_controller.TakeDamage();
        m_CurrFlashTime = Time.time;

        return true;
    }

}
