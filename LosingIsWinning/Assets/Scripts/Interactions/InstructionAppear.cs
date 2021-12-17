using System.Collections;
using UnityEngine;
using TMPro;

public class InstructionAppear : MonoBehaviour
{
    [TextArea(3, 10)] public string m_instructionText = "";
    public TextMeshPro m_textObj;
    public float m_printTime = 0.01f;

    private bool m_textPrinted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_textObj != null)
        {
            m_textObj.text = "";
        }

        m_textPrinted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_textPrinted)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        m_textPrinted = true;

        //make text appear
        StartCoroutine(MakeTextAppear());
    }

    IEnumerator MakeTextAppear()
    {
        foreach (char letter in m_instructionText)
        {
            m_textObj.text += letter;
            yield return new WaitForSeconds(m_printTime);
        }      
    }


}
