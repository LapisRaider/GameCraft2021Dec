using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonBase<DialogueManager>
{
    [Header("Dialogue UI")]
    public GameObject m_dialogueBox;
    public TextEffect m_text;
    public float m_textSpeed = 0.1f;

    [Header("Choices UI")]
    public GameObject[] m_choiceBoxButton;
    public TextEffect[] m_choiceBoxesText;

    [Header("Corruption text")]
    public float m_maxCorruptionFrequency = 0.5f;

    //TODO:: REMOVE THIS, TESTING PURPOESE
    public float TEMP_CORRUPTIONFREQUENCY = 0.0f;

    [Header("Text Data")]
    List<Dialogue> m_currDialogues;
    Dialogue m_currDiaOption; //updated when a new option has been picked
    DialogueText m_currText; //updated on new statement
    string m_fullNewDialogueText = ""; //got updated by the corruption also

    int m_currOptionIndex = 0;
    int m_currSentenceIndex = 0;
    int m_currCharIndex = 0;

    IEnumerator m_prevCouroutine = null;

    bool m_dialogueOver = true;

    //delegates
    [HideInInspector] public delegate void DialogueFinishDelegate(int lastDiOption);
    [HideInInspector] public DialogueFinishDelegate m_dialogueFinishCallback;

    [HideInInspector] public delegate void AngryAtPlayer();
    [HideInInspector] public AngryAtPlayer m_angryPlayerCallback;


    public void Start()
    {
        ResetDialogue();

        //make UI inactive
        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(false);

        if (m_text != null)
            m_text.gameObject.SetActive(false);

        m_dialogueOver = true;
    }

    public void Update()
    {
        if (!m_dialogueOver && Input.GetKeyDown(KeyCode.Return))
        {
            NextSentence();
        }
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
        m_fullNewDialogueText = "";

        //reset UI
        m_text.SetText("");
        m_text.SetTextEffect(DIALOGUE_EFFECTS.NONE);
        foreach (GameObject choiceBox in m_choiceBoxButton)
        {
            choiceBox.gameObject.SetActive(false);
        }
    }

    public void StartDialogue(DialogueData newDialogue, int currDialogueOption = 0)
    {
        ResetDialogue();

        //make UI active
        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(true);

        if (m_text != null)
            m_text.gameObject.SetActive(true);

        //update text data
        m_currDialogues = newDialogue.m_Dialogues;
        m_currOptionIndex = currDialogueOption;
        m_currDiaOption = m_currDialogues[currDialogueOption];
        m_currText = m_currDiaOption.m_dialogue[0];

        m_dialogueOver = false;

        //start printing
        StartPrintText();
    }

    public void NextSentence()
    {
        if (m_prevCouroutine != null)
            StopCoroutine(m_prevCouroutine);

        if (m_currDiaOption == null)
            return;

        if (m_currCharIndex < m_fullNewDialogueText.Length)
        {
            //print out the entire sentence
            m_currCharIndex = m_fullNewDialogueText.Length;
            m_text.SetText(m_fullNewDialogueText);
        }
        else if (m_currSentenceIndex >= m_currDiaOption.m_dialogue.Count - 1) //check if end
        {
            //or put out the options
            if (m_currDiaOption.m_hasChoice)
            {
                if (m_choiceBoxButton[0].activeSelf) //if already active dont bother
                    return;

                // put out options
                for (int i = 0; i < m_currDiaOption.m_choices.Count; ++i)
                {
                    m_choiceBoxButton[i].SetActive(true);
                    DialogueText choiceText = m_currDiaOption.m_choices[i].m_Text;

                    string textToPrint = new string(CorruptText(choiceText.m_text.ToCharArray()));
                    m_choiceBoxesText[i].SetText(textToPrint);
                    m_choiceBoxesText[i].SetTextEffect(choiceText.m_effect);
                }
            }
            else
            {
                CloseDialogue(); //close everything
            }
        }
        else //check if sentence is done printing out
        {
            //start next sentence printing
            ++m_currSentenceIndex;
            m_currText = m_currDiaOption.m_dialogue[m_currSentenceIndex];

            StartPrintText();
        }
    }

    public void StartPrintText()
    {
        m_text.SetTextEffect(m_currText.m_effect);

        char[] textCharArray = m_currText.m_text.ToCharArray();
        CorruptText(textCharArray);
        m_fullNewDialogueText = new string(textCharArray);

        m_prevCouroutine = PrintText(textCharArray);
        StartCoroutine(m_prevCouroutine);
    }

    public char[] CorruptText(char[] sentence)
    {
        //TODO:: should be from Math.Ceil(sentence.Length * Math.Clamp(1.0f - currSanity/fullsanity))
        //corruptionFrequency = Mathf.Clamp(corruptionFrequency, 0.0f, m_maxCorruption);

        float corruptionFrequency = Mathf.Ceil(sentence.Length * TEMP_CORRUPTIONFREQUENCY);
        for (int i = 0; i < (int)corruptionFrequency; ++i)
        {
            int randomIndex = Random.Range(0, sentence.Length);
            sentence[randomIndex] = (char)Random.Range(32, 127); //ascii code from ! to ~
        }

        return sentence;
    }

    public void PickOption(int buttonIndex)
    {
        //get next dialogue index
        if (m_currDiaOption == null)
            return;

        m_currOptionIndex = m_currDiaOption.m_choices[buttonIndex].m_nextDialogueIndex;

        //if -1, close dialogue box
        if (m_currOptionIndex == -1)
        {
            CloseDialogue();
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
        StartPrintText();
    }

    public void CloseDialogue()
    {
        if (m_currDiaOption.m_hitPlayerAfter)
        {
            if (m_angryPlayerCallback != null)
                m_angryPlayerCallback.Invoke();
        }

        foreach (GameObject choiceBox in m_choiceBoxButton)
        {
            choiceBox.gameObject.SetActive(false);
        }

        if (m_dialogueBox != null)
            m_dialogueBox.SetActive(false);

        if (m_text != null)
            m_text.gameObject.SetActive(false);

        if (m_dialogueFinishCallback != null)
            m_dialogueFinishCallback.Invoke(m_currOptionIndex);

        m_dialogueOver = true;
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
