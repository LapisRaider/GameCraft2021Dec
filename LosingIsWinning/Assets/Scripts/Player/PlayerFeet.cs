using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    public GameObject playerObject;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Testing falling on");

        if (collision.gameObject.CompareTag("Platform") && playerMovement.m_isGrounded == false && playerMovement.m_startJump == false)
        {
            Debug.Log("Testing falling on");
            playerMovement.resetJump();
            playerObject.transform.parent = collision.gameObject.transform;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Testing falling off");

        if (collision.gameObject.CompareTag("Platform"))
        {
            playerObject.transform.parent = null;
        }
    }
}
