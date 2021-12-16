using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy AI
public class GroundSlamEnemyController : MonoBehaviour
{
    public GROUNDSLAM_STATES m_currState;
    public enum GROUNDSLAM_STATES
    {
        STATE_NORMAL = 0,
        STATE_UNMORPHING,
        STATE_MORPHING,
        STATE_MORPHED_IDLE,
        STATE_MORPHED_PATROL,
        STATE_MORPHED_CHASE,
        STATE_MORPHED_ATTACKING,
        STATE_MORPHED_DEATH,
    }

    [Header("AI Variables")]
    public GameObject m_normalGO;
    public GameObject m_morphedGO;
    public GameObject m_attackGO;

    // Need to play test and change values accordingly
    static int HP = 1;
    static int DMG = 1;
    public int m_hp;
    public int m_dmg;

    // When attacktimer reaches attacktime, an attack will be made
    // Need to play test and change values accordingly
    static float ATT_TIME = 2;
    public float m_attackTimer;
    public float m_attackTime;

    [Header("Movement variables")]
    public Transform m_groundDetection;
    public float m_speed;
    [Tooltip("For raycasting")]
    public float m_distance;
    float m_patrolTimer;
    public float m_patrolTime;
    private bool m_movingRight;
    private bool m_moving;

    // Call this function when the player decides to lose sanity or when the duration ends
    public void SetMorphing(bool _morphing)
    {
        if (_morphing)
        {
            m_currState = GROUNDSLAM_STATES.STATE_MORPHING;
        }
        else
        {
            m_currState = GROUNDSLAM_STATES.STATE_UNMORPHING;
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
        m_currState = GROUNDSLAM_STATES.STATE_NORMAL;
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
            case GROUNDSLAM_STATES.STATE_NORMAL:
                break;
            case GROUNDSLAM_STATES.STATE_UNMORPHING:
                {
                    StartUnmorphing();
                }
                break;
            case GROUNDSLAM_STATES.STATE_MORPHING:
                {
                    StartMorphing();
                }
                break;
            case GROUNDSLAM_STATES.STATE_MORPHED_IDLE:
                {
                    //m_attackTimer += Time.deltaTime;

                    //if (m_attackTimer >= m_attackTime)
                    //{
                    //    m_attackTimer = 0;
                    //    m_currState = GROUNDSLAM_STATES.STATE_MORPHED_ATTACKING;
                    //    m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
                    //    //Attack();
                    //}
                    m_patrolTimer += Time.deltaTime;

                    if (m_patrolTimer >= m_patrolTime)
                    {
                        m_patrolTimer = 0;
                        // Toggle the moving bool
                        m_moving = !m_moving;
                    }
                }
                break;
            case GROUNDSLAM_STATES.STATE_MORPHED_PATROL:
                {
                    m_patrolTimer += Time.deltaTime;

                    if (m_patrolTimer >= m_patrolTime)
                    {
                        m_patrolTimer = 0;
                        // Toggle the moving bool
                        m_moving = !m_moving;
                    }
                }
                break;
            case GROUNDSLAM_STATES.STATE_MORPHED_CHASE:
                {

                }
                break; 
            case GROUNDSLAM_STATES.STATE_MORPHED_ATTACKING:
                break;
            default:
                break;
        }

        // If its patroling to move
        if (m_moving)
        {
            // Move 

        }
    }

    public void Attack()
    {
        m_attackGO.transform.position = transform.position;
        m_attackGO.SetActive(true);
        m_attackGO.GetComponent<GroundSlamEnemyAttack>().startAttack = true;
        m_currState = GROUNDSLAM_STATES.STATE_MORPHED_IDLE;
    }

    public void endAttack()
    {
        m_currState = GROUNDSLAM_STATES.STATE_MORPHED_IDLE;
        m_morphedGO.GetComponent<Animator>().SetBool("Attack", false);
        //m_morphedGO.GetComponent<Animator>()
    }    

    public void StartMorphing()
    {
        m_normalGO.SetActive(false);
        m_morphedGO.SetActive(true);

        // When done with morphing
        m_currState = GROUNDSLAM_STATES.STATE_MORPHED_IDLE;

    }

    public void StartUnmorphing()
    {
        m_attackTimer = 0;

        m_normalGO.SetActive(true);
        m_morphedGO.SetActive(false);
        m_attackGO.GetComponent<GroundSlamEnemyAttack>().ResetAll();

        // When done with unmorphing
        m_currState = GROUNDSLAM_STATES.STATE_NORMAL;
    }
}
