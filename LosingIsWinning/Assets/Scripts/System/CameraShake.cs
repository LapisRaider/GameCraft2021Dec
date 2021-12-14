using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource m_ShakeSource;

    private void Start()
    {
        m_ShakeSource = GetComponent<CinemachineImpulseSource>();
    }

    public void StartShake(float force = 1.0f)
    {
        m_ShakeSource.GenerateImpulse(force);
    }
}
