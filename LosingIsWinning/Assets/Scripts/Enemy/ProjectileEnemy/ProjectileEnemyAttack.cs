using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles attack
public class ProjectileEnemyAttack : MonoBehaviour
{
    public bool startAttack = false;
    public List<BoxCollider2D> m_hitboxes;
    public List<float> m_hitboxesLifetime;
    public List<Vector3> m_hitboxesDirection;

    // HITBOX_LIFETIME is max life time before projectile is gone
    // HITBOX_TRAVEL_SPEED is how fast the hitbox moves
    // Need to play test and change values accordingly
    static float HITBOX_LIFETIME = 6.0f; // The bigger the value,s the longer the hitbox will stay on the screen

    // Need to play test and change values accordingly
    static float HITBOX_TRAVEL_SPEED = 2;

    [System.NonSerialized] public GameObject playerObject;
    // m_hitboxDirection is the travel direction of the hitbox

    // Start is called before the first frame update
    void Start()
    {
        foreach (var hitbox in m_hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }
    }

    public void ResetAll()
    {
        startAttack = false;
        
        foreach (var hitbox in m_hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            // Create a new projectile
            for (int i = 0; i < m_hitboxes.Count; i++)
            {
                if (startAttack)
                {
                    if (!m_hitboxes[i].gameObject.activeSelf)
                    {
                        m_hitboxes[i].gameObject.SetActive(true);
                        m_hitboxes[i].gameObject.transform.position = transform.position;
                        m_hitboxesDirection[i] = (playerObject.transform.position - gameObject.transform.position).normalized;
                        m_hitboxesLifetime[i] = 0;
                        startAttack = false;
                    }

                    if (i == m_hitboxes.Count - 1 && startAttack)
                    {
                        float oldestProjectileLifetime = 0;
                        int oldestProjectileIndex = 0;

                        for (int j = 0; j < m_hitboxes.Count; j++)
                        {
                            if (m_hitboxesLifetime[j] > oldestProjectileLifetime)
                            {
                                oldestProjectileIndex = j;
                            }
                        }

                        m_hitboxes[oldestProjectileIndex].gameObject.SetActive(true);
                        m_hitboxes[oldestProjectileIndex].gameObject.transform.position = transform.position;
                        m_hitboxesDirection[oldestProjectileIndex] = (playerObject.transform.position - gameObject.transform.position).normalized;
                        m_hitboxesLifetime[oldestProjectileIndex] = 0;
                        startAttack = false;
                    }
                }
            }

        }
        // Loop through list of projectiles to look for active ones
        for (int i = 0; i < m_hitboxes.Count; i++)
        {
            if (m_hitboxes[i].gameObject.activeSelf)
            {
                m_hitboxesLifetime[i] += Time.deltaTime;

                if (m_hitboxesLifetime[i] >= HITBOX_LIFETIME)
                {
                    m_hitboxes[i].gameObject.SetActive(false);
                }
                else
                {
                    m_hitboxes[i].gameObject.transform.position += m_hitboxesDirection[i] * Time.deltaTime * HITBOX_TRAVEL_SPEED;
                }
            }
        }

    }
}