using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider m_slider;
    //public Rect
    public ParticleSystem m_particleEffect;

    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void SetHealth(int health)
    {
        if (m_slider != null)
            m_slider.value = health;
    }

    public void ShakeBar()
    {
        m_animator.SetTrigger("Shake");
    }
}