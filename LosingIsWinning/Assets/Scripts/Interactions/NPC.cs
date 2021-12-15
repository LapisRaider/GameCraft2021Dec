public class NPC : Interactiables
{
    public DialogueData m_NpcDialogue;
    private int m_lastDialogue = 0;

    public override void Interact()
    {
        DialogueManager.Instance.StartDialogue(m_NpcDialogue, m_lastDialogue);
        DialogueManager.Instance.m_dialogueFinishCallback += DialogueFinish;
    }

    public void DialogueFinish(int lastDialogueOption)
    {
        m_interactionFinish = true;
        m_lastDialogue = lastDialogueOption;
        DialogueManager.Instance.m_dialogueFinishCallback -= DialogueFinish;
    }
}
