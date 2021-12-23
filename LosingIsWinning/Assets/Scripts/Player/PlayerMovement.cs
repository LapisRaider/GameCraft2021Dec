using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Data")]
    public float m_walkSpeed = 100.0f;
    public float m_jumpSpeed = 400.0f;
    public float m_fallMultiplier = 2.5f; //how fast u fall

    public float m_dashSpeed = 200.0f;
    public float m_dashTime = 2.0f; //how long the dash lasts
    private float m_currDashTime = 0.0f; //keeps track of the time press dash
    public float m_dashCooldown = 1.0f;
    private float m_currDashCooldown = 0.0f; //how long before can dash again

    private float m_oriGravity = 0.5f; //original gravity

    private bool m_lockFaceDir = false; //cant change dir for visuals when fighting enemy
    private Vector2 m_inputDir = Vector2.zero;
    [HideInInspector] public Vector2 m_faceDir = Vector2.zero; //for animation, dash

    private Rigidbody2D m_rigidBody;
    private bool m_move = true;

    [Header("States")]
    [Tooltip("Standing on ground will reset jump")]
    public Transform m_groundCheckPos; 
    public Vector2 m_groundCheckSize;
    public LayerMask m_groundLayers;
    [System.NonSerialized] public bool m_isGrounded = true; //check if on ground

    private int m_currJumps = 0;
    [SerializeField] private int m_maxJumps = 2;
    [System.NonSerialized] public bool m_startJump = false; //when just press jump

    private int m_currDashes = 0;
    [System.NonSerialized] public bool m_isDashing = false;

    [Header("Combat")]
    public Vector2 m_hitOffset = Vector2.zero;
    public Vector2 m_hitEffectOffset = Vector2.zero;
    public Vector2 m_attackRange = Vector2.zero;
    public LayerMask m_attackObjectsMask;

    public float m_AttackRate = 1.0f;
    private float m_currAttackTime = 0.0f;

    private Vector3 m_hitDir = Vector2.zero;
    private Vector2 m_hitSize = Vector2.zero;

    public float m_bounceForce = 2.0f;
    private bool m_bounceUpAttack = false;


    [Header("Effects")]
    public float m_ghostFrequency = 0.2f;
    private float m_currGhostTime = 0.0f;
    private CameraShake m_CameraShake;

    private SpriteRenderer m_spriteRenderer;
    private Animator m_Animator;

    private bool m_AttackAnimDone = true;

    public SpriteRenderer m_slashRightFX;
    public GameObject m_slashUpFX;

    [Header("Damage Player")]
    public float m_invicibleTime = 0.2f;
    public float m_flickerTime = 0.1f;
    public Sprite m_hurtSprite;
    public float m_hurtSpriteTime = 0.1f; //how long to show hurt sprite

    [HideInInspector] public bool m_takeDamage = true;
    private float m_currInvincibleTime;

    private bool m_applyKnockBack = false;
    private Vector2 m_knockBackForce = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_CameraShake = Camera.main.GetComponent<CameraShake>();

        m_startJump = false;
        m_isDashing = false;
        m_isGrounded = Physics2D.OverlapBox(m_groundCheckPos.position, m_groundCheckSize, 0.0f, m_groundLayers);

        m_currDashCooldown = Time.time;
        m_currJumps = m_maxJumps;
        m_currAttackTime = Time.time;
        m_currGhostTime = Time.time;
        m_faceDir = new Vector2(1 , 0);

        m_oriGravity = m_rigidBody.gravityScale;

        m_Animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_AttackAnimDone = true;
        m_currInvincibleTime = Time.time;
        m_takeDamage = true;

        m_applyKnockBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //player got hit
        if (m_applyKnockBack)
            return;

        if (m_isDashing)
        {
            float currTime = Time.time;

            //for ghost effect
            if (currTime - m_currGhostTime >= m_ghostFrequency)
            {
                GameObject dashGhostObj = ObjectPooler.Instance.FetchGO("GhostPlayer");
                if (dashGhostObj != null)
                    dashGhostObj.transform.position = transform.position;
                m_currGhostTime = currTime;
            }

            //reset dash
            m_isDashing = currTime - m_currDashTime < m_dashTime;
            if (!m_isDashing)
            {
                m_rigidBody.gravityScale = m_oriGravity;
                m_currDashCooldown = Time.time;
            }
        }

        //do not move when camera still moving
        //if (!m_move)
        //{
        //    m_inputDir = Vector2.zero;
        //    UpdateAnimation();
        //    return;
        //}
         
        m_inputDir.x = Input.GetAxisRaw("Horizontal");
        m_inputDir.y = Input.GetAxisRaw("Vertical");

        //update for animation
        if (m_inputDir.x != 0.0f && !m_isDashing)
        {
            if (!m_lockFaceDir)
            {
                m_faceDir.x = m_inputDir.x;
            }
            m_spriteRenderer.flipX = m_faceDir.x < 0;
        }

        Combat();
        if (!m_AttackAnimDone) //check if attack anim not done
        {
            if (m_isGrounded)
            {
                m_inputDir.x = 0.0f; //dont let player move
            }
            CheckHitAnything();
            return;
        }

        //jump
        if (Input.GetButtonDown("Jump") && m_currJumps > 0)
        {
            --m_currJumps;
            m_startJump = true;
        }

        //for dashing
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !m_isDashing)
        {
            if (Time.time - m_currDashCooldown > m_dashCooldown)
            {
                m_isDashing = true;
                m_currDashTime = Time.time;
                m_currGhostTime = m_currDashTime;

                if (m_CameraShake != null)
                    m_CameraShake.StartShake();

                m_rigidBody.gravityScale = 0.0f; //so player doesnt fall
            }
        }

        UpdateVisuals();
    }

    private void FixedUpdate()
    {
        //press dash
        if (m_isDashing)
        {
            m_rigidBody.velocity = new Vector2(m_faceDir.x * m_dashSpeed * Time.fixedDeltaTime, 0.0f);
            return;
        }

        //got hit
        if (m_applyKnockBack)
        {
            m_rigidBody.velocity = m_knockBackForce;
            return;
        }

        //bounce upwards after successful hit
        if (m_bounceUpAttack)
        {
            //GameObject obj = ObjectPooler.Instance.FetchGO("ExplodeFX");
            //
            //if (obj != null)
            //    obj.transform.position = transform.position + m_hitDir;

            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, m_bounceForce);
            m_bounceUpAttack = false;
        }
        
        //check if land on ground
        m_isGrounded = Physics2D.OverlapBox(m_groundCheckPos.position, m_groundCheckSize, 0.0f, m_groundLayers);
        if (m_isGrounded) //reset number of jumps
        {
            if (m_currJumps < m_maxJumps && !m_startJump)
            {
                m_currJumps = m_maxJumps;
            }
        }

        //press jump
        if (m_startJump)
        {
            m_rigidBody.velocity = Vector2.up * m_jumpSpeed * Time.fixedDeltaTime;
            m_startJump = false;
        }

        //update right left movement
        m_rigidBody.velocity = new Vector2(m_inputDir.x * m_walkSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);

        //if falling
        if (m_rigidBody.velocity.y < 0.0f)
        {
            m_rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    //-------------------------------------------------Player Combat -----------------------------------------//
    public void Combat()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
            return;
       
        if (Time.time - m_currAttackTime < m_AttackRate)
            return;

        m_lockFaceDir = true;
        m_currAttackTime = Time.time;

        Vector3 hitDir = Vector3.zero;
        Vector2 hitSize = Vector2.zero;
        if (!m_isGrounded && m_inputDir.y < 0.0f) //is in air can hit down
        {
            hitDir.y = -m_hitOffset.y;
            hitSize = new Vector2(m_attackRange.y, m_attackRange.x);
        }
        else if (m_inputDir.y > 0.0f) //check if player want to hit up
        {
            hitDir.y = m_hitOffset.y;
            hitSize = new Vector2(m_attackRange.y, m_attackRange.x);

            if (m_slashUpFX != null)
            {
                m_slashUpFX.transform.position = transform.position + hitDir;
                m_slashUpFX.transform.position += new Vector3(0, m_hitEffectOffset.y);
                m_slashUpFX.SetActive(true);
            }
        }
        else if (m_inputDir.y < 0.0f) //check if player want to hit down
        {
            hitDir.y = -m_hitOffset.y;
            hitSize = new Vector2(m_attackRange.y, m_attackRange.x);
        }
        else //horizontal attack base on where the player is facing
        {
            hitDir.x = m_hitOffset.x * m_faceDir.x;
            hitSize = new Vector2(m_attackRange.x, m_attackRange.y);

            if (m_slashRightFX != null)
            {
                m_slashRightFX.flipX = m_faceDir.x < 0.0f;
                m_slashRightFX.gameObject.transform.position = transform.position + hitDir;
                m_slashRightFX.gameObject.transform.position += new Vector3(m_hitEffectOffset.x * m_faceDir.x, 0);
                m_slashRightFX.gameObject.SetActive(true);
            }
        }

        m_hitDir = hitDir;
        m_hitSize = hitSize;
        
        m_AttackAnimDone = false;
        UpdateAttackAnim();
    }

    public void CheckHitAnything()
    {
        //Collider2D[] attackObjs = Physics2D.OverlapBoxAll(transform.position + m_hitDir, m_hitSize, 0, m_attackObjectsMask);
        //foreach (Collider2D objs in attackObjs)
        //{
        //    HitObjs hitObj = objs.GetComponent<HitObjs>();
        //    if (hitObj == null)
        //        continue;
        //
        //    if (hitObj.Hit())
        //    {
        //        if (m_hitDir.y < 0.0f)
        //        {
        //            m_bounceUpAttack = true;
        //        }
        //    }
        //}
    }

    public void FinishAttackAnim()
    {
        m_lockFaceDir = false;
        m_AttackAnimDone = true;
    }
    //------------------------------------------------- End Player Combat -----------------------------------------//


    //------------------------------------------------- Player got hurt -----------------------------------------//
    public void PlayerKnockBack(Vector2 dir, float force = 1.0f)
    {
        if (!m_takeDamage)
            return;

        m_takeDamage = false;
        m_applyKnockBack = true;
        m_knockBackForce = dir * force;
        StartCoroutine(Invincibility());

        m_spriteRenderer.sprite = m_hurtSprite; //change sprite to temp hurt sprite
        m_Animator.enabled = false;
        Invoke("ChangeHurtSprite", m_hurtSpriteTime);
    }

    IEnumerator Invincibility()
    {
        m_currInvincibleTime = Time.time;
        while (Time.time - m_currInvincibleTime < m_invicibleTime)
        {
            //make player flicker in and out
            m_spriteRenderer.enabled = !m_spriteRenderer.enabled;

            yield return new WaitForSeconds(m_flickerTime);
        }

        m_spriteRenderer.enabled = true;
        m_takeDamage = true;

        yield return null;
    }

    public void ChangeHurtSprite()
    {
        m_Animator.enabled = true;
        m_applyKnockBack = false;
    }
    //------------------------------------------------- End Player got hurt -----------------------------------------

    public void UpdateAttackAnim()
    {
        if (Math.Abs(m_rigidBody.velocity.y) < Mathf.Epsilon) //not in air
        {
            if (m_inputDir.y < 0.0f) //crouch down attack
            {
                if (Math.Abs(m_rigidBody.velocity.y) < Mathf.Epsilon)
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Attack_Down));
                else
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Fall_Attack_Down));
            }
            else if (m_inputDir.y > 0.0f)
                m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Attack_Up));
            else //attack normally
                m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Attack_Hori));

            return;
        }
    }

    public void UpdateVisuals() //update animation and visuals of sprite
    {
        if (!m_AttackAnimDone)
            return;
        
        //fall
        if (m_rigidBody.velocity.y < 0.0f)
        {
            m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Fall));
            return;
        }
        else if (m_rigidBody.velocity.y > 0.0f) //jump
        {
            if (m_currJumps > 0 && m_currJumps < m_maxJumps) //initial first jump
                m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Jump));
            else //double jump
                m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_DoubleJump));

            return;
        }

        //dash
        if (m_isDashing)
        {
            m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Dash));
            return;
        }

        //default movement
        if (m_isGrounded)
        {
            if (Math.Abs(m_inputDir.x) > 0.0f) //moving
            {
                if (m_inputDir.y < 0.0f)
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Crouch_Move));
                else
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Walk));
            }
            else //not moving
            {
                if (m_inputDir.y < 0.0f)
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Crouch));
                else
                    m_Animator.Play(Enum.GetName(typeof(PlayerAnimations), PlayerAnimations.Player_Idle));
            }
        }
    }


    public void ResetMoveStates()
    {
        //reset dashing
        m_isDashing = false;
        m_rigidBody.gravityScale = m_oriGravity;

        //reset Movement
        m_inputDir = Vector2.zero;

        //reset attack
        m_AttackAnimDone = true;
        m_bounceUpAttack = false;
        m_lockFaceDir = false;

        //reset jump
        m_currJumps = m_maxJumps;
        m_startJump = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        if (m_groundCheckPos != null)
            Gizmos.DrawWireCube(m_groundCheckPos.position, m_groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(m_hitOffset.x, 0.0f), new Vector3(m_attackRange.x, m_attackRange.y, 1));
        Gizmos.DrawWireCube(transform.position + new Vector3(-m_hitOffset.x, 0.0f), new Vector3(m_attackRange.x, m_attackRange.y, 1));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, m_hitOffset.y), new Vector3(m_attackRange.y, m_attackRange.x, 1));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, -m_hitOffset.y), new Vector3(m_attackRange.y, m_attackRange.x, 1));


        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, -m_hitOffset.y), new Vector3(m_attackRange.y, m_attackRange.x, 1));
    }
}

public enum PlayerAnimations
{
    Player_Idle,
    Player_Walk,
    Player_Crouch,
    Player_Crouch_Move,

    Player_Jump,
    Player_DoubleJump,

    Player_Attack_Down,
    Player_Attack_Hori,
    Player_Attack_Up,
    Player_Fall_Attack_Down,

    Player_Death,
    Player_Fall,
    Player_Dash,
    Player_Hurt
}