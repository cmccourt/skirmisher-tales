using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isLevelCompleted;
    public static bool levelStarted;
    public AudioClip bGMusic;
    public int startGoldAmount;
    public int startOrbAmount;
    public GameObject levelCompPanel;
    public GameObject levelObjPanel;
    public GameObject gameOverPanel;
    public LayerMask finishTargetLayer;
    public GameObject[] dialogBoxes;
    private resourceManager resourceMan;
    private int dialogIndex = 0;
    private void Awake()
    {
        PauseMenu.isPaused = false;
        levelStarted = false;
        if(AudioManager.audioManager != null)
        {
            AudioManager.audioManager.PlayBackgroundMusic(bGMusic);
        }
    }

    void Start()
    {
        resourceManager.goldAmount = startGoldAmount;
        resourceManager.orbAmount = startOrbAmount;
        if (dialogBoxes.Length >0)
            StartDialog();
        else
            levelObjPanel.SetActive(true);
        resourceMan = FindObjectOfType<resourceManager>();
        isLevelCompleted = false;
        if (AudioManager.audioManager != null)
        {
            AudioManager.audioManager.PlayBackgroundMusic(bGMusic);
        }
        
    }

    private void Update()
    {
        if (isLevelCompleted)
        {
            PauseMenu.isPaused = true;
            
            levelCompPanel.SetActive(true);
        }
        if (GetComponent<Collider2D>() != null && GameObject.FindGameObjectsWithTag("Player").Length < 1)
        {
            if(resourceManager.goldAmount < 40)
            {
                GameOver();
            }
        }
    }

    public void Startlevel()
    {
        levelStarted = true;
        levelObjPanel.SetActive(false);
    }
    public void ChangeLevelStatus()
    {
        isLevelCompleted = true;
        AudioManager.audioManager.PlayVictoryMusic();
        print("LEVEL COMPLETE SUCCESS!!!");
    }

    public void GameOver()
    {
        isLevelCompleted = true;
        gameOverPanel.SetActive(true);
        print("GAME OVER!!!!");
    }

    public void QuitToMainMenu()
    {
        GameManager.gameManager.LoadLevelAsync("Main Menu");
    }

    public void LevelCompleted()
    {
        GameManager.gameManager.LevelCompleted();
    }

    public void StartDialog()
    {
        for(int i = 0; i < dialogBoxes.Length; i++)
        {
            dialogBoxes[i].SetActive(false);
        }
        dialogBoxes[dialogIndex].SetActive(true);
    }

    public void OpenNextDialog()
    {
        dialogBoxes[dialogIndex].SetActive(false);
        dialogIndex += 1;
        Debug.Log(dialogIndex);
        if(dialogIndex == dialogBoxes.Length)
        {
            Debug.Log("Dialog Done");
            levelObjPanel.SetActive(true);
        }
        else
        {
            dialogBoxes[dialogIndex].SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(finishTargetLayer == (finishTargetLayer | (1 << collision.gameObject.layer)))
        {
            ChangeLevelStatus();
        }
    }
}
