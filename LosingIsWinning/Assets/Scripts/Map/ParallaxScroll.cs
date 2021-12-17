using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float m_parallaxEffect = 0; //how much effect we want, further == lesser
    public int m_offsetObjects = 2; //number of image in the current layer
    public float m_distToChange = 100.0f;
    public float m_distOffset = 0.5f;

    public bool m_reuse = true;

    private float m_length;
    private float m_camStartPosX;
    private GameObject m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main.gameObject;

        m_camStartPosX = m_camera.transform.position.x;

        m_length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //follow the camera position by a certain offset in world
        float distMoved = m_camStartPosX - m_camera.transform.position.x;
        m_camStartPosX = m_camera.transform.position.x;
        transform.position = new Vector3(transform.position.x + (distMoved * m_parallaxEffect), transform.position.y, transform.position.z);

        if (!m_reuse)
            return;

        //check if background is too far from camera, if too far move it to a closer position
        float distFromCamera = m_camStartPosX - transform.position.x;
        float offset = m_length * m_offsetObjects;
        if (Mathf.Abs(distFromCamera) > m_distToChange)
        {
            float newPosX = distFromCamera < 0 ? transform.position.x - offset + m_distOffset : transform.position.x + offset - m_distOffset;
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        }
    }
}
