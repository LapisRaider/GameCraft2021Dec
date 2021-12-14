using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject m_dialogueBox;
    public TextMeshProUGUI m_text;

    DialogueData m_currDialogue;

    int m_currDialogueOption = 0;
    int m_currSentence = 0;
    int m_currCharIndex = 0;

    public void Start()
    {
        m_currDialogue = null;
        ResetDialogue();
    }

    public void ResetDialogue()
    {
        m_currDialogueOption = 0;
        m_currSentence = 0;
        m_currCharIndex = 0;
    }

    public void NextSentence()
    {
        //check if end, if yes close the UI

        //check if sentence is done printing out and no options out yet
            //start next sentence printing
            //or put out the options 
            

        //not done
            //print out the entire sentence
    }
}
