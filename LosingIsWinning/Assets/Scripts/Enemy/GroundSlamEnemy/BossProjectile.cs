using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    GroundSlamEnemyController m_controller;
    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponentInParent<GroundSlamEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 dir = Player.Instance.transform.position - transform.position;
            dir.Normalize();
            Player.Instance.HurtPlayer(dir, 1.0f, m_controller.m_dmg);
        }
    }
}
