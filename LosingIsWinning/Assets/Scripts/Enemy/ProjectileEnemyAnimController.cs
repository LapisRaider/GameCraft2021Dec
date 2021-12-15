using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAnimController : MonoBehaviour
{
    ProjectileEnemyController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<ProjectileEnemyController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runAttack()
    {
        controller.Attack();
    }

    public void endAttack()
    {
        //controller.endAttack();
    }

}
