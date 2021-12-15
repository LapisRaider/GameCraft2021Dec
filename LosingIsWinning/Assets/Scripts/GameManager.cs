using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    // Whenever sanity ability is used, m_SANITY_LOST_PER_CAST is amount of sanity lost
    // Need to play test and change values accordingly
    static int m_SANITY_LOST_PER_CAST = 1;


    public GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Testing if saving works
        if (Input.GetKey(KeyCode.V)) { SaveSystem.Instance.SaveTheGame(); }
        if (Input.GetKey(KeyCode.B)) { SaveSystem.Instance.LoadTheGame(); }
        if (Input.GetKey(KeyCode.B)) { SaveSystem.Instance.LoadTheGame(); }
    }

    public void UseSanityAbility()
    {
        PlayerData.Instance.m_currSanityMeter -= m_SANITY_LOST_PER_CAST;

    }
}
