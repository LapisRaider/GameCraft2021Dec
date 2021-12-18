using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    public GameObject[] m_storyImages;
    public List<CutSceneData> cutSceneText = new List<CutSceneData>();
    public float m_textSpeed = 0.2f;

    [Header("Scene transition")]
    public float m_TransitionTime = 1.0f;
    public string m_startSceneName = "GameScene";
    public Animator m_animator;

    IEnumerator m_prevCouroutine = null;

    private int m_currStoryImg = 0;
    private int m_currText = -1;

    [System.Serializable]
    public class CutSceneData
    {
        public TextMeshProUGUI m_textBoxObj;
        [TextArea(3, 10)] public string m_dialogue = "";
        public bool m_transitionNextScene = false;
    }

    public void Start()
    {
        for (int i = 0; i < cutSceneText.Count; ++i)
        {
            cutSceneText[i].m_textBoxObj.text = "";
        }

        SoundManager.Instance.Play("background");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextStory();
        }
    }

    public void NextStory()
    {
        //reset couroutine
        if (m_prevCouroutine != null)
        {
            StopCoroutine(m_prevCouroutine);
            m_prevCouroutine = null;
        }

        if (m_currText < 0)
        {
            PlayText();
            return;
        }

        if (m_currText >= cutSceneText.Count)
        {
            return;
        }

        if (cutSceneText[m_currText].m_textBoxObj.text.Length < cutSceneText[m_currText].m_dialogue.Length)
        {
            cutSceneText[m_currText].m_textBoxObj.text = cutSceneText[m_currText].m_dialogue;
        }
        else
        {
            if (cutSceneText[m_currText].m_transitionNextScene)
            {
                m_storyImages[m_currStoryImg].SetActive(false);
                ++m_currStoryImg;
                m_storyImages[m_currStoryImg].SetActive(true);
            }

            cutSceneText[m_currText].m_textBoxObj.gameObject.SetActive(false);
            PlayText();
        }
    }

    public void PlayText()
    {
        ++m_currText;
        if (m_currText >= cutSceneText.Count) //transition next scene
        {
            StartCoroutine(StartLoadGame());
            return;
        }
        else
        {
            cutSceneText[m_currText].m_textBoxObj.gameObject.SetActive(true);
        }

        m_prevCouroutine = PrintText();
        StartCoroutine(m_prevCouroutine);
    }

    IEnumerator PrintText()
    {
        cutSceneText[m_currText].m_textBoxObj.text = "";
        foreach (char letter in cutSceneText[m_currText].m_dialogue)
        {
            cutSceneText[m_currText].m_textBoxObj.text += letter;
            SoundManager.Instance.Play("type");

            yield return new WaitForSeconds(m_textSpeed);
        }

        yield return null;
    }

    IEnumerator StartLoadGame()
    {
        if (m_animator != null)
            m_animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(m_TransitionTime);

        SceneManager.LoadScene(m_startSceneName);
    }
}
