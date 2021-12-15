using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.m_player)
        {
            // Refill sanity
            PlayerData.Instance.m_currSanityMeter = PlayerData.Instance.m_maxSanityMeter;

            SaveSystem.Instance.SaveTheGame();
        }
    }
}   
