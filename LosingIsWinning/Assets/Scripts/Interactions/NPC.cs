using UnityEngine;

public class NPC : Interactiables
{
    [Header("Others")]
    public DialogueData m_NpcDialogue;
    public bool m_rememberPrevDia = false;
    public float m_hitForce = 1.0f;

    [Header("UI stuff")]
    public GameObject m_speechBox;

    [Header("Block Player")]
    public bool m_blockPlayer = false;
    public GameObject m_collider;
    private bool m_gotAngry = false;

    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    private int m_lastDialogue = 0;

    private float m_dirX;

    [Header("HACKS")]
    public bool m_givePowerup = false;

    public string m_soundName = "Talking1";

    public override void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_dirX = 1.0f;
    }

    public override void Interact()
    {
        //look at player
        m_dirX = Player.Instance.transform.position.x - gameObject.transform.position.x;
        if (m_spriteRenderer != null)
            m_spriteRenderer.flipX = m_dirX > 0;

        DialogueManager.Instance.StartDialogue(m_NpcDialogue, m_rememberPrevDia && !m_blockPlayer ? m_lastDialogue : 0);
        DialogueManager.Instance.m_dialogueFinishCallback += DialogueFinish;
        DialogueManager.Instance.m_angryPlayerCallback += NPCAngry;

        m_gotAngry = false;
        m_interactionFinish = false;

        SoundManager.Instance.Play(m_soundName);
    }

    public void DialogueFinish(int lastDialogueOption)
    {
        m_interactionFinish = true;
        m_lastDialogue = lastDialogueOption;
        DialogueManager.Instance.m_dialogueFinishCallback -= DialogueFinish;
        DialogueManager.Instance.m_angryPlayerCallback -= NPCAngry;

        if (!m_gotAngry && m_blockPlayer)
        {
            m_collider.SetActive(false);
            m_blockPlayer = false;

            //THIS IS A HACK, REMOVE IF CAN
            if (m_givePowerup)
            {
                //TODO:: CALL ALLOW PLAYER EAT CATNIP
                m_givePowerup = false;
                PlayerData.Instance.m_sanityAbility = 1;
                PlayerData.Instance.m_maxDashes = 1;
            }
        }
    }

    public void NPCAngry()
    {
        //smack player
        Player.Instance.HurtPlayer(new Vector2(m_dirX, 1.0f), m_hitForce);
        m_gotAngry = true;

        if (m_animator != null)
            m_animator.SetTrigger("Attack");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_speechBox != null)
            m_speechBox.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (m_speechBox != null)
            m_speechBox.SetActive(false);
    }
}
