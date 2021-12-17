using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyAnimController : MonoBehaviour
{
    DashEnemyController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<DashEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runAttack()
    {
        // 2nd one lmao
        controller.Attack();
    }

    public void endAttack()
    {
        controller.EndAttack();
    }

    public void Dead()
    {
        controller.Dead();
    }

}
