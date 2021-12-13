using UnityEngine;

public class Door : MonoBehaviour
{
    public Room m_CurrRoom;
    public Room m_NextRoom;

    private float m_currTime;
    private float m_offsetTime = 0.1f;

    private void OnEnable()
    {
        m_currTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - m_currTime <= m_offsetTime)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        PlayerData.Instance.EnableActions(false);
        m_NextRoom.gameObject.SetActive(true);
        m_NextRoom.RoomActivation(true);
        m_CurrRoom.RoomActivation(false);
    }
}
