using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : SingletonBase<PlayerData>
{
    // Whenever you add new data variable, make sure that save
    // system is also gonna save the new data.
    // Modify under SaveFile.cs

    public int m_maxJumps;
    public int m_maxSanityMeter;
    public int m_currSanityMeter;
    public int m_sanityAbility;
    public bool m_isInsane;

    [Header("Curr States")]
    private bool m_move = true;

    // Start is called before the first frame update
    void Start()
    {
        m_maxJumps = 1;
        m_maxSanityMeter = 15;
        m_currSanityMeter = m_maxSanityMeter;
        m_sanityAbility = 1;
        m_isInsane = false;
    }

    public void EnableActions(bool enable)
    {
        m_move = enable;
    }

    public bool CanMove()
    {
        return m_move;
    }
}
