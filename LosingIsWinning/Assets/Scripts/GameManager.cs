using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    // Whenever sanity ability is used, m_SANITY_LOST_PER_CAST is amount of sanity lost
    // Need to play test and change values accordingly
    static int m_SANITY_LOST_PER_CAST = 1;

    public GameObject m_player;
    // How long the sanity will last for one use.
    public float m_SanityTimer = 45.0f;
    // Used to keep track of the time
    public float m_CurrentSanityTimer = 0.0f;    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.Instance.m_isInsane)
        {
            m_CurrentSanityTimer -= Time.deltaTime;
            if (m_CurrentSanityTimer <= 0.0f)
            {
                PlayerData.Instance.m_isInsane = false;
                Healthbar.Instance.InsaneMode(false);
                Player.Instance.SetInsane(false);
                m_CurrentSanityTimer = 0.0f;
            }
        }
        // Testing if saving works
        if (Input.GetKey(KeyCode.V)) { SaveSystem.Instance.SaveTheGame(); }
        if (Input.GetKey(KeyCode.B)) { SaveSystem.Instance.LoadTheGame(); }
        if (Input.GetKey(KeyCode.B)) { SaveSystem.Instance.LoadTheGame(); }
        if (Input.GetKey(KeyCode.E)) { UseSanityAbility(); }
    }

    public void UseSanityAbility()
    {
        if (PlayerData.Instance.m_sanityAbility == 0)
        {
            return;
        }


        // Check if it isnt enabled
        if (!PlayerData.Instance.m_isInsane)
        {
            Healthbar.Instance.InsaneMode(true);
            ShockWaveFX.Instance.StartShockWave();
            Player.Instance.SetInsane(true);

            // Deduct the sanity meter and set the timer
            TakeSanityDamage(m_SANITY_LOST_PER_CAST, true);
            //PlayerData.Instance.m_currSanityMeter -= m_SANITY_LOST_PER_CAST;
            //// Check if they lost
            //if (PlayerData.Instance.m_currSanityMeter <= 0)
            //{
            //    // If they did
            //    //idk what happens 
            //}
            m_CurrentSanityTimer = m_SanityTimer;
            PlayerData.Instance.m_isInsane = true;
        }
        else
        {
            // Im not sure if they use it when its already on, will it reset the timer and drain the sanity
            // Or will it not be allowed to be used until the timer is up 
            //Healthbar.Instance.InsaneMode(false);
        }
    }

    public void TakeSanityDamage(int sanityLost, bool sanityAbility = false)
    {
        PlayerData.Instance.m_currSanityMeter -= sanityLost;

        if (PlayerData.Instance.m_currSanityMeter <= 0)
        {
            // Lost again
        }

        Healthbar.Instance.SetHealth((float)(PlayerData.Instance.m_currSanityMeter) / (float)(PlayerData.Instance.m_maxSanityMeter), sanityAbility);
        Healthbar.Instance.LoseHealth();
    }

    public void GainSanity(int sanityGain)
    {
        PlayerData.Instance.m_currSanityMeter += sanityGain;

        // Make sure no overflow.
        if (PlayerData.Instance.m_currSanityMeter >= PlayerData.Instance.m_maxSanityMeter)
        {
            PlayerData.Instance.m_currSanityMeter = PlayerData.Instance.m_maxSanityMeter;
        }

        Healthbar.Instance.SetHealth(PlayerData.Instance.m_currSanityMeter / PlayerData.Instance.m_maxSanityMeter);
        Healthbar.Instance.GainHealth();
    }
}
