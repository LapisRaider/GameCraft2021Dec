using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public BoxCollider2D m_endGameTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        EndTheGame();
    }

    public void EndTheGame()
    {
        SceneManager.LoadScene("EndCutScene");
    }
}
