using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public GameObject pauseMenuGO;

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (!LevelManager.isLevelCompleted)
            {
                pauseMenuGO.SetActive(true);
            }
        }
        else
        {
            pauseMenuGO.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
    }

    public void ReturnToMainMenu()
    {
        GameManager.gameManager.LoadLevelAsync("Main Menu");
    }
}
