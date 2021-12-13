using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
    public GameObject[] m_roomDoors;
    public GameObject m_RoomVirtualCamera;

    private bool m_activate = true;

    [Header("Camera Movement")]
    private CinemachineBrain m_ChineMachineBrain;
    private bool m_checkBlend = false;

    public void Start()
    {
        m_ChineMachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        m_checkBlend = false;
        m_activate = true;
    }

    public void FixedUpdate()
    {
        if (!m_checkBlend)
            return;

        if (!m_ChineMachineBrain.IsBlending)
        {
            //when blend finish, set the proper room inactive, tell player they can move now
            gameObject.SetActive(m_activate);
            PlayerData.Instance.EnableActions(true);
            m_checkBlend = false;
        }
    }

    public void RoomActivation(bool activate)
    {
        //set all doors to active
        foreach (GameObject door in m_roomDoors)
        {
            door.SetActive(activate);
        }

        //turn camera active
        m_RoomVirtualCamera.SetActive(activate);

        m_activate = activate;
        m_checkBlend = true;
    }
}
