using UnityEngine;

public class ReplayEffects : MonoBehaviour
{
    public string m_stateName = "effects";

    Animator m_animator;

    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_animator.Play(m_stateName, -1, 0.0f);
    }

    public void FinishAnim()
    {
        gameObject.SetActive(false);
    }
}
