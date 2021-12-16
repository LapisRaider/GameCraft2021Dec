using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    public List<DialogueText> m_dialogue = new List<DialogueText>();

    public bool m_hasChoice = false;
    public List<DialogueChoice> m_choices = new List<DialogueChoice>();

    public bool m_hitPlayerAfter = false;
}

[System.Serializable]
public class DialogueText
{
    [TextArea(3, 10)]
    public string m_text = "";
    public DIALOGUE_EFFECTS m_effect = DIALOGUE_EFFECTS.NONE;

    //can add name/title/image here too if we want, or more bool to say its player or not
}

[System.Serializable]
public class DialogueChoice
{
    public string name;

    public int m_nextDialogueIndex = -1; //if its -ve 1 means nothing
    public DialogueText m_Text;
}

public enum DIALOGUE_EFFECTS
{
    NONE,
    WOBBLY,
    SHAKING
}
