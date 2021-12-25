using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : SingletonBase<SanityManager>
{
    // Not sure how many levels of insanity we will have
    public enum Sanity_State
    {
        Level1,
        Level2,
        Level3
    };

    [System.NonSerialized] public Sanity_State m_playerSanity;
    // Will the different sanity level have different timings?
    public float m_sanityTimer = 30.0f;
    float m_sanityTime;
    bool m_sanity;


    public override void Awake()
    {
        base.Awake();

        m_playerSanity = Sanity_State.Level1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is insane mode
        if (m_sanity)
        {
            m_sanityTime += Time.deltaTime; 
            if (m_sanityTime >= m_sanityTimer)
            {
                m_sanity = false;
            }
        }
    }

    public void ToggleSanity()
    {
        // If anything happens when player toggles sanity
        // i.e taking damage?
        switch(m_playerSanity)
        {
            case Sanity_State.Level1:
                {

                    break;
                }
            case Sanity_State.Level2:
                {
                    
                    break;
                }
            case Sanity_State.Level3:
                {

                    break;
                }
        }

    
    }

    // Not sure if we're doing the upgrade of catnip or the ability to swap between the catnip
    // atm this is just to change the value
    public void ChangeSanity(Sanity_State sanity)
    {
        m_playerSanity = sanity;
    }
}
