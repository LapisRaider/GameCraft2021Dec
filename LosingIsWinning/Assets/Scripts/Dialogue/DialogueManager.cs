using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject m_dialogueBox;
    public TextEffect m_text;
    public float m_textSpeed = 0.1f;

    [Header("Choices UI")]
    public GameObject[] m_choiceBoxButton;
    public TextEffect[] m_choiceBoxesText;

    //TODO:: REMOVE THIS, TESTING PURPOESE
    public DialogueData testDialogueData;

    [Header("Text Data")]
    List<Dialogue> m_currDialogues;
    Dialogue m_currDiaOption; //updated when a new option has been picked
    DialogueText m_currText; //updated on new statement

    int m_currOptionIndex = 0;
    int m_currSentenceIndex = 0;
    int m_currCharIndex = 0;

    IEnumerator m_prevCouroutine = null;

    public void Start()
    {
        ResetDialogue();
    }

    public void ResetDialogue()
    {
        //reset couroutine
        if (m_prevCouroutine != null)
            StopCoroutine(m_prevCouroutine);

        m_prevCouroutine = null;
       
        //reset data
        m_currDialogues = null;
        m_currDiaOption = null;
        m_currText = null;

        m_currOptionIndex = 0;
        m_currSentenceIndex = 0;
        m_currCharIndex = 0;

        //reset UI
        m_text.SetText("");
        m_text.SetTextEffect(DIALOGUE_EFFECTS.NONE);
        foreach (GameObject choiceBox in m_choiceBoxButton)
        {
            choiceBox.gameObject.SetActive(false);
        }
    }

    //TODO:: REMOVE THIS, THIS IS FOR TESTING PURPOSES
    public void TESTING()
    {
        ResetDialogue();

        //make UI active
        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(true);

        if (m_text != null)
            m_text.gameObject.SetActive(true);

        m_currDialogues = testDialogueData.m_Dialogues;
        m_currDiaOption = m_currDialogues[0];
        m_currText = m_currDiaOption.m_dialogue[0];

        m_text.SetTextEffect(m_currText.m_effect);
        m_prevCouroutine = PrintText(m_currText.m_text.ToCharArray());
        StartCoroutine(m_prevCouroutine);
    }

    public void StartDialogue(DialogueData newDialogue)
    {
        ResetDialogue();

        //make UI active
        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(true);

        if (m_text != null)
            m_text.gameObject.SetActive(true);

        //update text data
        m_currDialogues = newDialogue.m_Dialogues;
        m_currDiaOption = m_currDialogues[0];
        m_currText = m_currDiaOption.m_dialogue[0];

        //start printing
        m_text.SetTextEffect(m_currText.m_effect);
        m_prevCouroutine = PrintText(m_currText.m_text.ToCharArray());
        StartCoroutine(m_prevCouroutine);
    }

    public void NextSentence()
    {
        if (m_prevCouroutine != null)
            StopCoroutine(m_prevCouroutine);

        if (m_currCharIndex < m_currText.m_text.Length)
        {
            //print out the entire sentence
            m_currCharIndex = m_currText.m_text.Length;
            m_text.SetText(m_currText.m_text);
        }
        else if (m_currSentenceIndex >= m_currDiaOption.m_dialogue.Count - 1) //check if end
        {
            //or put out the options
            if (m_currDiaOption.m_hasChoice)
            {
                // put out options
                for (int i = 0; i < m_currDiaOption.m_choices.Count; ++i)
                {
                    m_choiceBoxButton[i].SetActive(true);
                    DialogueText choiceText = m_currDiaOption.m_choices[i].m_Text;

                    m_choiceBoxesText[i].SetText(choiceText.m_text);
                    m_choiceBoxesText[i].SetTextEffect(choiceText.m_effect);
                }
            }
            else
            {
                //close everything
                CloseDialogue();
            }
        }
        else //check if sentence is done printing out
        {
            //start next sentence printing
            ++m_currSentenceIndex;
            m_currText = m_currDiaOption.m_dialogue[m_currSentenceIndex];

            m_text.SetTextEffect(m_currText.m_effect);
            m_prevCouroutine = PrintText(m_currText.m_text.ToCharArray());
            StartCoroutine(m_prevCouroutine);

            //TODO
            //if guy is high, change some of the chars in the text to make it more warped
        }
    }

    public void PickOption(int buttonIndex)
    {
        //get next dialogue index
        m_currOptionIndex = m_currDiaOption.m_choices[buttonIndex].m_nextDialogueIndex;

        //if -1, close dialogue box
        if (m_currOptionIndex == -1)
        {
            CloseDialogue();
            //TODO:: can do a smacking also 

            return;
        }

        //reset things to the next text
        m_currDiaOption = m_currDialogues[m_currOptionIndex];
        m_currSentenceIndex = 0;
        m_currText = m_currDiaOption.m_dialogue[m_currSentenceIndex];
        
        foreach (GameObject choiceBox in m_choiceBoxButton)
        {
            choiceBox.gameObject.SetActive(false);
        }

        //update the next option and play the starting sentence of the next dialogue
        m_text.SetTextEffect(m_currText.m_effect);
        m_prevCouroutine = PrintText(m_currText.m_text.ToCharArray());
        StartCoroutine(m_prevCouroutine);
    }

    public void CloseDialogue()
    {
        foreach (GameObject choiceBox in m_choiceBoxButton)
        {
            choiceBox.gameObject.SetActive(false);
        }

        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(false);

        if (m_text != null)
            m_text.gameObject.SetActive(false);
    }

    IEnumerator PrintText(char[] sentence)
    {
        if (m_text == null || sentence == null)
            yield break;

        m_text.SetText("");
        m_currCharIndex = 0;
        foreach (char letter in sentence)
        {
            m_text.AddCharToText(letter);
            ++m_currCharIndex;

            yield return new WaitForSeconds(m_textSpeed);
        }

        yield return null;
    }
}
