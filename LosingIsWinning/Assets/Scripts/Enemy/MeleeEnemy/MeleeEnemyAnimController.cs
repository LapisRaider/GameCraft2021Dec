using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimController : MonoBehaviour
{
    MeleeEnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<MeleeEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runAttack()
    {
        // 2nd one lmao
        enemyController.Attack();
    }

    public void endAttack()
    {
        enemyController.EndAttack();
    }
}
