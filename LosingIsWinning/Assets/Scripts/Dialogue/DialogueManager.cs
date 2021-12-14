using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject m_dialogueBox;
    public TextMeshProUGUI m_text;
    public float m_textSpeed = 0.1f;

    List<Dialogue> m_currDialogues;

    Dialogue m_currDiaOption;
    DialogueText m_currText;

    int m_currOptionIndex = 0;
    int m_currSentenceIndex = 0;
    int m_currCharIndex = 0;

    public void Start()
    {
        m_currDialogues = null;
        ResetDialogue();
    }

    public void ResetDialogue()
    {
        m_currDialogues = null;

        m_currDiaOption = null;
        m_currText = null;

        m_currOptionIndex = 0;
        m_currSentenceIndex = 0;
        m_currCharIndex = 0;
    }

    public void StartDialogue(DialogueData newDialogue)
    {
        ResetDialogue();

        m_currDialogues = newDialogue.m_Dialogues;
        m_currDiaOption = m_currDialogues[0];
        m_currText = m_currDiaOption.m_dialogue[0];
    }

    public void NextSentence()
    {
        StopCoroutine(PrintText(null));

        if (m_currCharIndex < m_currText.m_text.Length)
        {
            //print out the entire sentence
            m_currCharIndex = m_currText.m_text.Length;
            m_text.text = m_currText.m_text;
        }
        else if (m_currCharIndex >= m_currDiaOption.m_dialogue.Count) //check if end
        {
            //or put out the options
            if (m_currDiaOption.m_hasChoice)
            {
                // put out options
            }
            else
            {
                //close everything
            }
        }
        else //check if sentence is done printing out
        {
            //start next sentence printing
            ++m_currSentenceIndex;
            m_currText = m_currDiaOption.m_dialogue[m_currSentenceIndex];
            StartCoroutine(PrintText(m_currText.m_text.ToCharArray()));

            //TODO
            //if guy is high, change some of the chars in the text to make it more warped
        }


        //when option is picked
        //set currDialogueOption, reset currSentence and currCharIndex
    }

    IEnumerator PrintText(char[] sentence)
    {
        if (m_text == null || sentence == null)
            yield break;

        m_text.text = null;
        foreach (char letter in sentence)
        {
            m_text.text += letter;
            ++m_currCharIndex;

            yield return new WaitForSeconds(m_textSpeed);
        }

        yield return null;
    }
}
