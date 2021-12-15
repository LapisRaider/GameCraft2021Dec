using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float m_parallaxEffect = 0; //how much effect we want, further == lesser

    private float m_length;
    private float m_startPosX;

    private GameObject m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main.gameObject;
        m_startPosX = transform.position.x;

        m_length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //check if background is too far from camera, if too far move it to a closer position
        float distFromCamera = m_camera.transform.position.x * (1 - m_parallaxEffect);
        if (distFromCamera > m_startPosX + m_length)
        {
            m_startPosX += m_length;
        }
        else if (distFromCamera < m_startPosX - m_length)
        {
            m_startPosX -= m_length;
        }

        //follow the camera position by a certain offset in world
        float distMoved = m_camera.transform.position.x * m_parallaxEffect;
        transform.position = new Vector3(m_startPosX + distMoved, transform.position.y, transform.position.z);
    }
}
