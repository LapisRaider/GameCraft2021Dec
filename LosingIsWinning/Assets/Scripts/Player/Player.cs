using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBase<Player>
{
    [Header("Animation stuff")]
    Animator m_playerAnimator;
    public RuntimeAnimatorController m_insaneAnimator;
    public RuntimeAnimatorController m_normalAnimator;
    public RuntimeAnimatorController m_deadAnimator;
    public float m_deathTime = 2.0f;

    Interactiables m_currInteraction;
    bool m_InteractionStarted = false;

    PlayerMovement m_movement;

    // Start is called before the first frame update
    public override void Awake()
    {
        m_currInteraction = null;
        m_movement = GetComponent<PlayerMovement>();
        m_playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InteractionStarted)
        {
            if (m_currInteraction == null || m_currInteraction.InteractionFinish())
            {
                PlayerData.Instance.EnableActions(true);
                m_InteractionStarted = false;
            }

            return;
        }

        //if player in interaction trigger and press interact key
        if (m_currInteraction != null && Input.GetKeyDown(KeyCode.Return))
        {
            m_currInteraction.Interact();
            PlayerData.Instance.EnableActions(false); //make player unable to move
            m_InteractionStarted = true;
        }
    }

    public void SetInsane(bool insane)
    {
        if (insane)
            m_playerAnimator.runtimeAnimatorController = m_insaneAnimator;
        else
            m_playerAnimator.runtimeAnimatorController = m_normalAnimator;
    }

    public void HurtPlayer(Vector2 dir, float force = 1.0f, int sanityDamage = 0)
    {
        if (!m_movement.m_takeDamage)
            return;

        m_movement.PlayerKnockBack(dir, force);
        GameManager.Instance.TakeSanityDamage(sanityDamage);
    }

    public bool GetPlayerFaceDir()
    {
        return m_movement.m_faceDir.x < 0;
    }

    public void PlayerDied()
    {
        m_playerAnimator.runtimeAnimatorController = m_deadAnimator;
        PlayerData.Instance.EnableActions(false);
        Invoke("ResetDeath", m_deathTime);
    }

    public void ResetDeath()
    {
        m_playerAnimator.runtimeAnimatorController = m_normalAnimator;
        PlayerData.Instance.EnableActions(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            m_currInteraction = collision.gameObject.GetComponent<Interactiables>();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = transform.position - collision.transform.position;
            dir.Normalize();
            HurtPlayer(dir, 1.0f, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_currInteraction != null && collision.gameObject == m_currInteraction.gameObject)
        {
            m_currInteraction = null;
        }
    }
}
