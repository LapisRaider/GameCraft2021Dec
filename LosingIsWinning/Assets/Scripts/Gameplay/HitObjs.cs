using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjs : MonoBehaviour
{
    [Header("Sprite Effects")]
    public Color m_HitColor = Color.red;
    public float m_HitFlashTime = 0.2f;

    private float m_CurrFlashTime = 0.0f;
    private bool m_HasBeenHit = false;

    SpriteRenderer m_spriteRenderer;

    public void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (!m_HasBeenHit)
            return;

        if (Time.time - m_CurrFlashTime > m_HitFlashTime)
        {
            m_spriteRenderer.color = Color.white;
            m_HasBeenHit = false;
        }
    }

    public bool Hit()
    {
        if (m_HasBeenHit)
        {
            return false;
        }

        if (m_spriteRenderer == null)
            return false;

        m_HasBeenHit = true;
        m_CurrFlashTime = Time.time;

        m_spriteRenderer.color = m_HitColor;

        return true;
    }
}
