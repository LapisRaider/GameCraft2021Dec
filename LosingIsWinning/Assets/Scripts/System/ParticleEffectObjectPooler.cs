using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectObjectPooler : SingletonBase<ParticleEffectObjectPooler>
{
    public List<GameObject> m_prefabs; 
    public int m_amtToCreate = 2;

    List<List<ParticleSystem>> m_particleEffectPooler = new List<List<ParticleSystem>>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_prefabs.Count; ++i)
        {
            m_particleEffectPooler.Add(new List<ParticleSystem>());
            CreateMore(m_amtToCreate, i);
        }
    }

    void CreateMore(int amt, int particleIndex)
    {
        for (int i = 0; i < amt; ++i)
        {
            GameObject obj = Instantiate(m_prefabs[particleIndex]);
            obj.transform.parent = transform;
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            m_particleEffectPooler[particleIndex].Add(particle);
        }
    }

    public void PlayParticle(Vector3 pos, PARTICLE_EFFECT_TYPE effect)
    {
        int effectIndex = (int)effect;
        if (effectIndex >= m_particleEffectPooler.Count)
            return;

        for (int i = 0; i < m_particleEffectPooler[effectIndex].Count; ++i)
        {
            if (!m_particleEffectPooler[effectIndex][i].isPlaying)
            {
                m_particleEffectPooler[effectIndex][i].transform.position = pos;
                m_particleEffectPooler[effectIndex][i].Play();
                return;
            }
        }

        CreateMore(m_amtToCreate, effectIndex);
        PlayParticle(pos, effect);
    }
}

public enum PARTICLE_EFFECT_TYPE
{
    JUMP,
}
