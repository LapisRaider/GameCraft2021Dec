using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles attack
public class MeleeEnemyAttack : MonoBehaviour
{
    public bool startAttack = false;
    public List<BoxCollider2D> m_hitboxes;

    // When m_hitboxTimer reaches m_hitboxTime, the melee hitbox will disappear
    // Need to play test and change values accordingly
    static float HITBOX_LIFETIME = 0.1f; // The bigger the value, the longer the hitbox will stay on the screen
    public float m_hitboxTimer;
    public float m_hitboxTime;

    public float m_attackRange;
    [System.NonSerialized] public bool m_movingRight;
    MeleeEnemyController m_controller;
    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponentInParent<MeleeEnemyController>();
       // Debug.Log("RANGE OF ATTACK" + m_attackRange);
       // m_attackRange = GetComponentInParent<MeleeEnemyController>().m_attackRange;
       // m_movingRight = GetComponentInParent<MeleeEnemyController>().m_movingRight;
       //m_hitboxTimer = HITBOX_LIFETIME;
       //m_hitboxTime = HITBOX_LIFETIME;

        //foreach (var hitbox in m_hitboxes)
        //{
        //    hitbox.gameObject.SetActive(false);
        //}
    }

    public void ResetAll()
    {
        startAttack = false;

        //m_hitboxTimer = HITBOX_LIFETIME;
        //m_hitboxTime = HITBOX_LIFETIME;

        //foreach (var hitbox in m_hitboxes)
        //{
        //    hitbox.gameObject.SetActive(false);
        //}

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startAttack)
        {
            if (m_hitboxTimer >= m_hitboxTime)
            {
                //Debug.Log("RANGE OF ATTACK" + m_attackRange);
                // Debug.Log("In Attack script " + m_movingRight);
                Vector3 centerPosition = transform.position - new Vector3(0, 0.5f, 0);

                RaycastHit2D[] m_attack;
                if (m_movingRight)
                {
                   // Debug.Log("Test right");
                    m_attack = Physics2D.RaycastAll(centerPosition, Vector2.right, m_attackRange);
                    Debug.DrawRay(centerPosition, (Vector2.right * m_attackRange), Color.green);
                   
                }
                else
                {
                    //Debug.Log("Test left");
                    m_attack = Physics2D.RaycastAll(centerPosition, Vector2.left, m_attackRange);
                    Debug.DrawRay(centerPosition, (Vector2.left * m_attackRange), Color.green);

                }

                foreach (RaycastHit2D hit in m_attack)
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        // GameManager.Instance.TakeSanityDamage(m_controller.m_dmg);
                        Vector3 dir = Player.Instance.transform.position - transform.position;
                        dir.Normalize();
                        Player.Instance.HurtPlayer(dir, 1.0f, m_controller.m_dmg);
                        break;
                    }
                }


                startAttack = false;
                ResetAll();
            }
            else
            {
                m_hitboxTimer += Time.deltaTime;
            }
        }

        //if (startAttack)
        //{
        //    if (m_hitboxTimer >= m_hitboxTime)
        //    {
        //        foreach (var hitbox in m_hitboxes)
        //        {
        //            if (!hitbox.gameObject.activeSelf)
        //            {
        //                m_hitboxTimer = 0;

        //                hitbox.gameObject.SetActive(true);
        //                return;
        //            }
        //        }

        //        // If all hit boxes are generated
        //        startAttack = false;
        //        ResetAll();
        //    }
        //    else
        //    {
        //        m_hitboxTimer += Time.deltaTime;
        //    }
        //}
    }
}
