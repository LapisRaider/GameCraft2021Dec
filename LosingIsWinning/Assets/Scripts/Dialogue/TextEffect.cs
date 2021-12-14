using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [Header("Wobble Effect")]
    public Vector2 m_wobbleMagnitude = Vector2.zero;

    [Header("Shake Effect")]
    public Vector2 m_shakeMagnitude = Vector2.zero;

    DIALOGUE_EFFECTS m_currEffect = DIALOGUE_EFFECTS.NONE;

    TMP_Text m_textMesh;
    TextMeshProUGUI m_text;
    Mesh m_mesh;
    Vector3[] m_vertices;

    // Start is called before the first frame update
    void Awake()
    {
        m_textMesh = GetComponent<TMP_Text>();
        m_text = GetComponent<TextMeshProUGUI>();
    }

    public void SetTextEffect(DIALOGUE_EFFECTS effect)
    {
        m_currEffect = effect;
    }

    public void SetText(string text)
    {
        if (m_text == null)
            return;

        m_text.text = text;
    }

    public void AddCharToText(char letter)
    {
        if (m_text == null)
            return;

        m_text.text += letter;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currEffect == DIALOGUE_EFFECTS.NONE)
            return;

        m_textMesh.ForceMeshUpdate();
        m_mesh = m_textMesh.mesh;
        m_vertices = m_mesh.vertices;

        for (int i = 0; i < m_textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = m_textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = Vector3.zero;
            if (m_currEffect == DIALOGUE_EFFECTS.WOBBLY)
                offset = Wobble(Time.time + i);
            else if (m_currEffect == DIALOGUE_EFFECTS.SHAKING)
                offset = Shake();


            m_vertices[index] += offset;
            m_vertices[index + 1] += offset;
            m_vertices[index + 2] += offset;
            m_vertices[index + 3] += offset;
        }

        m_mesh.vertices = m_vertices;
        m_textMesh.canvasRenderer.SetMesh(m_mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * m_wobbleMagnitude.x), Mathf.Cos(time * m_wobbleMagnitude.y));
    }

    Vector2 Shake()
    {
        return new Vector2(Random.Range(0.0f, 1.0f) * m_shakeMagnitude.x, Random.Range(0.0f, 1.0f) * m_shakeMagnitude.y);
    }
}
