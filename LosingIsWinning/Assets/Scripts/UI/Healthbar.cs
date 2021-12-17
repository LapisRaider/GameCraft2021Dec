using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : SingletonBase<Healthbar>
{
    public Slider m_slider;
    public ParticleSystem m_dmgParticleEffect;

    [Header("Bar filling")]
    public Color m_insaneBarColor;
    public Color m_normalBarColor;
    public Image m_barFillUI;
    Color m_currBarColor;

    [Header("Red Buffer")]
    public Slider m_redBuffSlider;
    private float m_oriRefBuffAmt = 1.0f;
    private float m_currDiff = 1.0f;
    private bool m_startPowerup = false;

    [Header("Colors Eyes")]
    public Image m_eyesUI;
    public Color m_insaneEyes;
    public Color m_normalEyes;
    public Color m_painEyes;
    public float m_eyesFlashRate = 0.2f;
    public float m_flashAmt = 0.5f;

    Color m_currEyes;

    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_currEyes = m_normalEyes;
        m_currBarColor = m_normalBarColor;
        m_animator = GetComponent<Animator>();

        if (m_dmgParticleEffect != null)
        {
            ParticleSystem.MainModule main = m_dmgParticleEffect.main;
            main.startColor = m_currBarColor;
        }
    }

    private void Update()
    {
        if (!m_startPowerup)
            return;

        if (m_redBuffSlider == null || m_slider == null)
            return;

        m_redBuffSlider.value = m_oriRefBuffAmt - m_currDiff * ( 1.0f - (GameManager.Instance.m_CurrentSanityTimer / GameManager.Instance.m_SanityTimer));
        m_redBuffSlider.value = Mathf.Clamp(m_redBuffSlider.value, m_slider.value, 1.0f);
        m_startPowerup = m_redBuffSlider.value > m_slider.value;
    }

    public void InsaneMode(bool insane)
    {
        if (insane)
        {
            m_currEyes = m_insaneEyes;
            m_currBarColor = m_insaneBarColor;
        }
        else
        {
            m_currEyes = m_normalEyes;
            m_currBarColor = m_normalBarColor;
        }

        if (m_eyesUI != null)
            m_eyesUI.color = m_currEyes;

        if (m_eyesUI != null)
            m_barFillUI.color = m_currBarColor;

        if (m_dmgParticleEffect != null)
        {
            ParticleSystem.MainModule main = m_dmgParticleEffect.main;
            main.startColor = m_currBarColor;
        }
    }

    public void SetHealth(float health, bool powerup = false)
    {
        if (m_slider != null)
            m_slider.value = health;

        if (m_redBuffSlider == null)
            return;

        if (powerup)
        {
            m_oriRefBuffAmt = m_redBuffSlider.value;
            m_currDiff = m_redBuffSlider.value - m_slider.value;
            m_startPowerup = true;
        }
        else if (!m_startPowerup)
        {
            m_redBuffSlider.value = health; //just update health normally
        }
    }

    public void LoseHealth()
    {
        StopCoroutine(EyesBlink());

        if (m_animator != null)
            m_animator.SetTrigger("Damage");

        if (m_dmgParticleEffect != null)
            m_dmgParticleEffect.Play();

        StartCoroutine(EyesBlink());
    }

    public void GainHealth()
    {
        //m_animator.SetTrigger("Shake");
    }

    IEnumerator EyesBlink()
    {
        float currTime = Time.time;
        bool switchEyes = true;

        while (Time.time - currTime < m_flashAmt)
        {
            if (m_eyesUI != null)
                m_eyesUI.color = switchEyes ? m_painEyes : m_currEyes;
            switchEyes = !switchEyes;
            yield return new WaitForSeconds(m_eyesFlashRate);
        }

        if (m_eyesUI != null)
            m_eyesUI.color = m_currEyes;
    }
}