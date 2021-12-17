using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy AI
public class ProjectileEnemyController : MonoBehaviour
{
    public PROJECTILE_STATES m_currState;
    public enum PROJECTILE_STATES
    {
        STATE_NORMAL = 0,
        STATE_UNMORPHING,
        STATE_MORPHING,
        STATE_MORPHED_IDLE,
        STATE_MORPHED_ATTACKING,
        STATE_MORPHED_DEATH,
    }

    public GameObject m_normalGO;
    public GameObject m_morphedGO;
    public GameObject m_attackGO;

    // Need to play test and change values accordingly
    static int HP = 1;
    static int DMG = 1;
    public int m_hp;
    public int m_dmg;


    [Header("Attack attributes")]
    // When attacktimer reaches attacktime, an attack will be made
    // Need to play test and change values accordingly
    static float ATT_TIME = 2;
    public float m_attackTime;
    //public float m_attackRange;
    float m_attackTimer;
    GameObject m_player;
    bool m_attacking = false;


    // Call this function when the player decides to lose sanity or when the duration ends
    public void SetMorphing(bool _morphing)
    {
        if (_morphing)
        {
            m_currState = PROJECTILE_STATES.STATE_MORPHING;
        }
        else
        {
            m_currState = PROJECTILE_STATES.STATE_UNMORPHING;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hp = HP;
        m_dmg = DMG;

        m_attackTimer = 0;
        m_attackTime = ATT_TIME;

        m_normalGO.SetActive(true);
        m_morphedGO.SetActive(false);
        m_attackGO.SetActive(false);
        m_currState = PROJECTILE_STATES.STATE_NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        // Testing purposes
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetMorphing(true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetMorphing(false);
        }


        switch (m_currState)
        {
            case PROJECTILE_STATES.STATE_NORMAL:
                break;
            case PROJECTILE_STATES.STATE_UNMORPHING:
                {
                    StartUnmorphing();
                }
                break;
            case PROJECTILE_STATES.STATE_MORPHING:
                {
                    StartMorphing();
                    m_morphedGO.GetComponent<Animator>().SetBool("Morph", true);
                }
                break;
            case PROJECTILE_STATES.STATE_MORPHED_IDLE:
                {
                    //m_attackTimer += Time.deltaTime;

                    //if (m_attackTimer >= m_attackTime)
                    //{
                    //    m_attackTimer = 0;
                    //    m_currState = PROJECTILE_STATES.STATE_MORPHED_ATTACKING;
                    //    m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
                    //    //Attack();
                    //}
                }
                break;
            case PROJECTILE_STATES.STATE_MORPHED_ATTACKING:
                {
                    if (m_player != null)
                    {
                        m_attackGO.GetComponent<ProjectileEnemyAttack>().playerObject = m_player;
                    }
                    else
                    {
                        m_attackGO.GetComponent<ProjectileEnemyAttack>().playerObject = null;
                    }

                    m_attackTimer += Time.deltaTime;

                    if (m_attackTimer >= m_attackTime)
                    {
                        m_attackTimer = 0;
                        m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Attack()
    {
        m_attackGO.transform.position = transform.position;
        m_attackGO.SetActive(true);
        m_attackGO.GetComponent<ProjectileEnemyAttack>().startAttack = true;
       // m_currState = PROJECTILE_STATES.STATE_MORPHED_IDLE;
    }

    public void EndAttack()
    {
        // Player is no longer in range
        if(m_player == null)
        {
            m_currState = PROJECTILE_STATES.STATE_MORPHED_IDLE;
            m_morphedGO.GetComponent<Animator>().SetBool("Attack", false);
            m_attackTimer = 0;
        }
    }

    public void StartMorphing()
    {
        m_normalGO.SetActive(false);
        m_morphedGO.SetActive(true);

        // When done with morphing
        m_currState = PROJECTILE_STATES.STATE_MORPHED_IDLE;

    }

    public void StartUnmorphing()
    {
        m_attackTimer = 0;

        m_normalGO.SetActive(true);
        m_morphedGO.SetActive(false);
        m_attackGO.GetComponent<ProjectileEnemyAttack>().ResetAll();

        // When done with unmorphing
        m_currState = PROJECTILE_STATES.STATE_NORMAL;
    }

    public void TakeDamage()
    {
        Debug.Log("HIT");
        m_morphedGO.GetComponent<Animator>().SetTrigger("Hit");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_currState = PROJECTILE_STATES.STATE_MORPHED_ATTACKING;

            m_player = collision.gameObject;
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_player = null;
            Debug.Log("Player not in range");
        }
    }
}
