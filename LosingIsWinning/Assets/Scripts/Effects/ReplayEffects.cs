using UnityEngine;

public class ReplayEffects : MonoBehaviour
{
    Animator m_animator;

    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_animator.Play("effects", -1, 0.0f);
    }

    public void FinishAnim()
    {
        gameObject.SetActive(false);
    }
}
