using UnityEngine;

public class NPC : Interactiables
{
    public DialogueData m_NpcDialogue;
    private Animator m_animator;

    private int m_lastDialogue = 0;

    public override void Interact()
    {
        DialogueManager.Instance.StartDialogue(m_NpcDialogue, m_lastDialogue);
        DialogueManager.Instance.m_dialogueFinishCallback += DialogueFinish;
        m_interactionFinish = false;
    }

    public void DialogueFinish(int lastDialogueOption)
    {
        m_interactionFinish = true;
        m_lastDialogue = lastDialogueOption;
        DialogueManager.Instance.m_dialogueFinishCallback -= DialogueFinish;
    }

    public void NPCAngry()
    {
        //TODO::
        //smack player
        //get an instance to player or something to do so
        if (m_animator != null)
            m_animator.SetTrigger("Attack");
    }
}
