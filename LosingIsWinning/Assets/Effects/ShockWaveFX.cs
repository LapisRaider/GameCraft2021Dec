using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveFX : MonoBehaviour
{
    public Material m_ShockWaveMaterial;
    public float m_ShockWaveSize = 0.1f;
    public float m_ShockWaveMagnification = -0.75f;
    public float m_StopThreshold = 0.9f; //should be from 0 - 1

    public float m_ShockWaveUpdateSpeed = 1.0f;
    private float m_CurrShockWaveOffset = 0.0f;
    private Resolution m_prevResolution;

    // Start is called before the first frame update
    void Start()
    {
        ResetShockWave();
        ResetResolution();
    }

    public void Update()
    {
        if (m_prevResolution.width != Screen.currentResolution.width || m_prevResolution.height != Screen.currentResolution.height)
            ResetResolution();

        if (Input.GetButtonDown("Jump"))
        {
            StartShockWave();
        }
    }

    public void StartShockWave()
    {
        StopCoroutine("UpdateShockWave");

        m_ShockWaveMaterial.SetFloat("Size", m_ShockWaveSize);
        m_ShockWaveMaterial.SetFloat("Magnification", m_ShockWaveMagnification);
        m_ShockWaveMaterial.SetFloat("shockwaveOffset", 0.0f);
        StartCoroutine("UpdateShockWave");
    }

    public void ResetShockWave()
    {
        if (m_ShockWaveMaterial == null)
            return;

        m_ShockWaveMaterial.SetFloat("Size", 0.0f);
        m_ShockWaveMaterial.SetFloat("Magnification", 0.0f);
        m_ShockWaveMaterial.SetFloat("shockwaveOffset", m_StopThreshold);

        m_CurrShockWaveOffset = 0.0f;
    }

    public void ResetResolution()
    {
        m_prevResolution = Screen.currentResolution;

        float resolution = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (m_ShockWaveMaterial != null)
            m_ShockWaveMaterial.SetFloat("SizeRatio", resolution);
    }

    IEnumerator UpdateShockWave()
    {
        while (m_CurrShockWaveOffset < m_StopThreshold)
        {
            m_CurrShockWaveOffset += m_ShockWaveUpdateSpeed * Time.deltaTime;
            m_ShockWaveMaterial.SetFloat("shockwaveOffset", m_CurrShockWaveOffset);
            
            yield return null;
        }

        ResetShockWave();
    }
}
