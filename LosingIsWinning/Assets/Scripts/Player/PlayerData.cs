using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : SingletonBase<PlayerData>
{
    [Header("Curr States")]
    private bool m_move = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
