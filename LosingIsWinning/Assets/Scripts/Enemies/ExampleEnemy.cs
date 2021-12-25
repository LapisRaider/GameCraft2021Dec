using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEnemy : EnemyBase
{
    #region Animation-related hashes
    int moving_bool;        // bool for isWalking
    int hit_trigger;        // Take damage trigger
    int attack_trigger;     // Shoot trigger
    int attack_animation;   // Attack animation
    int death_trigger;      // Death trigger
    #endregion
    // Start is called before the first frame update
    override public void Awake()
    {
        base.Awake();

    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
    }

    override public bool TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if (health > 0)
        {
          
        }
        return true;
    }


    override protected void OnDeath()
    {
        base.OnDeath();
    }
}
