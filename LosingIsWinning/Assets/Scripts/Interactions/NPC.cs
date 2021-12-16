using UnityEngine;

public class NPC : Interactiables
{
    public DialogueData m_NpcDialogue;
    public bool m_rememberPrevDia = false;
    public float m_hitForce = 1.0f;

    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    private int m_lastDialogue = 0;

    private float m_dirX;

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

        DialogueManager.Instance.StartDialogue(m_NpcDialogue, m_rememberPrevDia ? m_lastDialogue : 0);
        DialogueManager.Instance.m_dialogueFinishCallback += DialogueFinish;
        DialogueManager.Instance.m_angryPlayerCallback += NPCAngry;

        m_interactionFinish = false;
    }

    public void DialogueFinish(int lastDialogueOption)
    {
        m_interactionFinish = true;
        m_lastDialogue = lastDialogueOption;
        DialogueManager.Instance.m_dialogueFinishCallback -= DialogueFinish;
        DialogueManager.Instance.m_angryPlayerCallback -= NPCAngry;
    }

    public void NPCAngry()
    {
        //smack player
        Player.Instance.HurtPlayer(new Vector2(m_dirX, 1.0f), m_hitForce);

        if (m_animator != null)
            m_animator.SetTrigger("Attack");
    }
}
