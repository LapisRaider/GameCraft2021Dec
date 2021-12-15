using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Animator m_MainMenuAnimator;

    [Header("Scene transition")]
    public float m_TransitionTime = 1.0f;
    public string m_startSceneName = "AngieTest";

    // Start is called before the first frame update
    void Start()
    {
        //TODO::play the sound
    }

    public void StartGame()
    {
        StartCoroutine(StartLoadGame());
    }

    IEnumerator StartLoadGame()
    {
        if (m_MainMenuAnimator != null)
            m_MainMenuAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(m_TransitionTime);

        SceneManager.LoadScene(m_startSceneName);
    }
}
