using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBase<Player>
{
    Interactiables m_currInteraction;
    bool m_InteractionStarted = false;

    PlayerMovement m_movement;

    // Start is called before the first frame update
    public override void Awake()
    {
        m_currInteraction = null;
        m_movement = GetComponent<PlayerMovement>();
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

    public void HurtPlayer(Vector2 dir, float force = 1.0f, int sanityDamage = 0)
    {
        m_movement.PlayerKnockBack(dir, force);
        GameManager.Instance.TakeSanityDamage(sanityDamage);
    }

    public bool GetPlayerFaceDir()
    {
        return m_movement.m_faceDir.x < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            m_currInteraction = collision.gameObject.GetComponent<Interactiables>();
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
