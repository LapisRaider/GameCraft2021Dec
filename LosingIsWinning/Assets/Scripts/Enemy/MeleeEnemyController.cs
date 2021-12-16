using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy AI
public class MeleeEnemyController : MonoBehaviour
{
    public MELEE_STATES m_currState;
    public enum MELEE_STATES
    {
        STATE_NORMAL = 0,
        STATE_UNMORPHING,
        STATE_MORPHING,
        STATE_MORPHED_IDLE,
        STATE_MORPHED_CHASE,
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
    [Tooltip("How long they patrol and idle for")]
    public float m_patrolTime;
    [Header("Player Detection")]
    public float m_detectionRange;
    public float m_attackRange;

    bool m_movingRight = true;
    bool m_moving;
    float m_patrolTimer;


    // Call this function when the player decides to lose sanity or when the duration ends
    public void SetMorphing(bool _morphing)
    {
        if (_morphing)
        {
            m_currState = MELEE_STATES.STATE_MORPHING;
        }
        else
        {
            m_currState = MELEE_STATES.STATE_UNMORPHING;
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
        m_currState = MELEE_STATES.STATE_NORMAL;
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
            case MELEE_STATES.STATE_NORMAL:
                break;
            case MELEE_STATES.STATE_UNMORPHING:
                {
                    StartUnmorphing();
                }
                break;
            case MELEE_STATES.STATE_MORPHING:
                {
                    StartMorphing();
                }
                break;
            case MELEE_STATES.STATE_MORPHED_IDLE:
                {
                    //m_attackTimer += Time.deltaTime;

                    //if (m_attackTimer >= m_attackTime)
                    //{
                    //    m_attackTimer = 0;
                    //    m_currState = MELEE_STATES.STATE_MORPHED_ATTACKING;
                    //    m_morphedGO.GetComponent<Animator>().SetBool("Attack", true); 
                    //}

                    m_patrolTimer += Time.deltaTime;

                    if (m_patrolTimer >= m_patrolTime)
                    {
                        m_patrolTimer = 0;
                        // Toggle the moving bool
                        m_moving = !m_moving;
                    }
                    CheckForPlayer();
                    IdleMovement();
                }
                break;
            case MELEE_STATES.STATE_MORPHED_CHASE:
                {
                    CheckForPlayer();
                    ChasingMovement();
                }
                break;
            case MELEE_STATES.STATE_MORPHED_ATTACKING:
                m_attackTimer += Time.deltaTime;

                if (m_attackTimer >= m_attackTime)
                {
                    m_attackTimer = 0;
                    m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
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
        m_attackGO.GetComponent<MeleeEnemyAttack>().startAttack = true;
        //m_currState = MELEE_STATES.STATE_MORPHED_CHASE;
    }

    public void EndAttack()
    {
        m_currState = MELEE_STATES.STATE_MORPHED_CHASE;
        m_morphedGO.GetComponent<Animator>().SetBool("Attack", false);
        m_attackTimer = 0;
    }

    public void CheckForPlayer()
    {
        RaycastHit2D hitInfoRight = Physics2D.Raycast(transform.position, Vector2.right, m_detectionRange, ~gameObject.layer);
        RaycastHit2D hitInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, m_detectionRange, ~gameObject.layer);

        Debug.DrawRay(transform.position, (Vector2.right * m_detectionRange), Color.red);
        Debug.DrawRay(transform.position, (Vector2.left * m_detectionRange), Color.red);
        if (hitInfoRight.collider == true)
        {
            Debug.Log(hitInfoRight.collider.gameObject.name);

            if (hitInfoRight.collider.gameObject.tag == "Player")
            {
                m_currState = MELEE_STATES.STATE_MORPHED_CHASE;
                m_patrolTimer = 0.0f;
                RotateRight();
            }
        }
        else if (hitInfoLeft.collider == true)
        {
            Debug.Log(hitInfoLeft.collider.gameObject.name);

            if (hitInfoLeft.collider.gameObject.tag == "Player")
            {
                m_currState = MELEE_STATES.STATE_MORPHED_CHASE;
                m_patrolTimer = 0.0f;
                RotateLeft();
            }
        }
    }

    public void IdleMovement()
    {
        if (m_moving)
        {
            transform.Translate(Vector2.right * m_speed * Time.deltaTime);

            // Get the bottom and right hit info 
            // Checks if theres a wall infront or a drop below
            RaycastHit2D hitInfoDown = Physics2D.Raycast(m_groundDetection.position, Vector2.down, m_distance);
            RaycastHit2D hitInfoForward;
            if (m_movingRight)
            {
                hitInfoForward = Physics2D.Raycast(m_groundDetection.position, Vector2.right, m_distance);
                //Debug.DrawRay(m_groundDetection.position, (Vector2.right * m_distance), Color.green);

            }
            else
            {
                hitInfoForward = Physics2D.Raycast(m_groundDetection.position, Vector2.left, m_distance);

                //Debug.DrawRay(m_groundDetection.position, (Vector2.left * m_distance), Color.green);
            }
            // Down collider did not hit anything
            // If forward collider hit something, it means there is smth infront of it
            if (hitInfoForward.collider == true && hitInfoForward.collider.gameObject.tag == "Map")
            {
                if (m_movingRight)
                {
                    RotateLeft();
                }
                else
                {
                    RotateRight();
                }
            }
            else if (hitInfoDown.collider == false)
            {
                if (m_movingRight)
                {
                    RotateLeft();
                }
                else
                {
                    RotateRight();
                }
            }
        }

    }

    public void ChasingMovement()
    {

        RaycastHit2D hitInfoRight = Physics2D.Raycast(transform.position, Vector2.right, m_attackRange, ~gameObject.layer);
        RaycastHit2D hitInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, m_attackRange, ~gameObject.layer);
      
        if (hitInfoRight.collider == true && hitInfoRight.collider.gameObject.tag == "Player")
        {
            // if they are facing left
            if (m_movingRight == false)
            {
                RotateRight();
            }

            m_currState = MELEE_STATES.STATE_MORPHED_ATTACKING;
            m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
            m_patrolTimer = 0.0f;
            m_moving = false;
        }
        else if (hitInfoLeft.collider == true && hitInfoLeft.collider.gameObject.tag == "Player")
        {
            // If they facing left
            if (m_movingRight == true)
            {
                RotateLeft();
            }

            m_currState = MELEE_STATES.STATE_MORPHED_ATTACKING;
            m_morphedGO.GetComponent<Animator>().SetBool("Attack", true);
            m_patrolTimer = 0.0f;
            m_moving = false;
        }
        else
        {
            // no player in range
            m_moving = true;
        }

        // else just move towards the player
        if (m_moving)
        {
            transform.Translate(Vector2.right * m_speed * Time.deltaTime);

            // Get the bottom and right hit info 
            // Checks if theres a wall infront or a drop below
            RaycastHit2D hitInfoDown = Physics2D.Raycast(m_groundDetection.position, Vector2.down, m_distance);
            RaycastHit2D hitInfoForward;
            if (m_movingRight)
            {
                hitInfoForward = Physics2D.Raycast(m_groundDetection.position, Vector2.right, m_distance);
                //Debug.DrawRay(m_groundDetection.position, (Vector2.right * m_distance), Color.green);

            }
            else
            {
                hitInfoForward = Physics2D.Raycast(m_groundDetection.position, Vector2.left, m_distance);

                //Debug.DrawRay(m_groundDetection.position, (Vector2.left * m_distance), Color.green);
            }
            // Down collider did not hit anything
            // If forward collider hit something, it means there is smth infront of it
            // If it did hit then it means the player ran
            if (hitInfoForward.collider == true && hitInfoForward.collider.gameObject.tag == "Map")
            {
                m_currState = MELEE_STATES.STATE_MORPHED_IDLE;
                m_patrolTimer = 0.0f;
                m_moving = false;

            }
            else if (hitInfoDown.collider == false)
            {
                m_currState = MELEE_STATES.STATE_MORPHED_IDLE;
                m_patrolTimer = 0.0f;
                m_moving = false;

            }
        }

    }

    public void RotateLeft()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);
        m_movingRight = false;
    }

    public void RotateRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        m_movingRight = true;
    }

    public void StartMorphing()
    {
        m_normalGO.SetActive(false);
        m_morphedGO.SetActive(true);

        // When done with morphing
        m_currState = MELEE_STATES.STATE_MORPHED_IDLE;
    }

    public void StartUnmorphing()
    {
        m_attackTimer = 0;

        m_normalGO.SetActive(true);
        m_morphedGO.SetActive(false);
        m_attackGO.GetComponent<MeleeEnemyAttack>().ResetAll();

        // When done with unmorphing
        m_currState = MELEE_STATES.STATE_NORMAL;
    }
}
