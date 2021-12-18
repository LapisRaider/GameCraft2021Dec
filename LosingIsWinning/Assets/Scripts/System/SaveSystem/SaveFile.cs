using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public string m_currLevel;
    public int m_maxJumps;
    public int m_maxDashes;
    public int m_maxSanityMeter;
    public int m_currSanityMeter;
    public int m_sanityAbility;
    public bool m_isInsane;
    public float[] m_pos;
}