using UnityEngine;

public class GhostPlayerEffect : MonoBehaviour
{
    public float m_alphaSpeed = 0.2f;
    public float m_startingAlpha = 0.8f;
    private float m_currAlpha = 0.0f;

    SpriteRenderer m_sprite;

    private void Awake()
    {
        m_currAlpha = m_startingAlpha;
        m_sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        m_currAlpha = m_startingAlpha;
        //m_sprite.flipX = Player.Instance.GetPlayerFaceDir(); //set to curr player face dir
    }

    private void Update()
    {
        m_currAlpha -= Time.deltaTime * m_alphaSpeed;
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, m_currAlpha);
        if (m_sprite.color.a <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
