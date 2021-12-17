using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EnlargeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public Vector3 m_enlargeSize = new Vector3(2,2,2);
    public RectTransform m_transform;

    private Vector3 m_oriSize;

    void Start()
    {
        m_oriSize = m_transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_transform.localScale = m_enlargeSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_transform.localScale = m_oriSize;
    }
}
