using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : SingletonBase<SaveSystem>
{
    public List<GameObject> m_levels;
    
    public void SaveTheGame()
    {
        SaveFile saveFile = new SaveFile();

        saveFile.m_currLevel = GameManager.Instance.m_currLevel;
        saveFile.m_maxJumps = PlayerData.Instance.m_maxJumps;
        saveFile.m_maxDashes = PlayerData.Instance.m_maxDashes;
        saveFile.m_maxSanityMeter = PlayerData.Instance.m_maxSanityMeter;
        saveFile.m_currSanityMeter = PlayerData.Instance.m_currSanityMeter;
        saveFile.m_sanityAbility = PlayerData.Instance.m_sanityAbility;
        saveFile.m_isInsane = false;
        saveFile.m_pos = new float[3];
        saveFile.m_pos[0] = GameManager.Instance.m_player.transform.position.x;
        saveFile.m_pos[1] = GameManager.Instance.m_player.transform.position.y;
        saveFile.m_pos[2] = GameManager.Instance.m_player.transform.position.z;


        // Convert SaveData into a txt file
        BinarySaveFileFormatter.SavePlayer(saveFile);
    }

    public void LoadTheGame()
    {
        // Open txt file and get data
        SaveFile data = BinarySaveFileFormatter.LoadPlayer();

        // Use the data
        PlayerData.Instance.m_maxSanityMeter = data.m_maxSanityMeter;
        PlayerData.Instance.m_currSanityMeter = data.m_maxSanityMeter;
        PlayerData.Instance.m_maxJumps = data.m_maxJumps;
        PlayerData.Instance.m_maxDashes = data.m_maxDashes;
        PlayerData.Instance.m_sanityAbility = data.m_sanityAbility;
        PlayerData.Instance.m_isInsane = data.m_isInsane;
        GameManager.Instance.m_player.transform.position = new Vector3(data.m_pos[0], data.m_pos[1], data.m_pos[2]);


        foreach (var level in m_levels)
        {
            level.SetActive(false);

            if (level.name == data.m_currLevel)
            {
                level.SetActive(true);
            }
        }
        Healthbar.Instance.SetHealth(PlayerData.Instance.m_currSanityMeter / PlayerData.Instance.m_maxSanityMeter);
        Healthbar.Instance.InsaneMode(false);
    }
}
