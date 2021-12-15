using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Interactiables m_currInteraction;
    bool m_InteractionStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        m_currInteraction = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InteractionStarted)
        {
            if (m_currInteraction == null || m_currInteraction.InteractionFinish())
            {
                PlayerData.Instance.EnableActions(true);
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
