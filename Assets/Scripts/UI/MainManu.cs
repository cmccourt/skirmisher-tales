using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainManu : MonoBehaviour {

    public AudioClip bGMusic;
    public GameObject overwritePanel;
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.audioManager.PlayBackgroundMusic(bGMusic);
        PlayerData saveFile = GameManager.gameManager.LoadSaveFile();
        Debug.Log(saveFile);
        if (saveFile != null)
        {
            newGameButton.onClick.RemoveListener(() => GameManager.gameManager.FadeToLevel("Level Select"));
            newGameButton.onClick.AddListener(() => EnableOverwritePanel());
            loadGameButton.onClick.AddListener(() => GameManager.gameManager.FadeToLevel("Level Select"));
        }
        else
        {
            newGameButton.onClick.AddListener(() => GameManager.gameManager.CreateNewSaveFile());
            loadGameButton.interactable = false;
        }
        
        settingsButton.onClick.AddListener(() => GameManager.gameManager.FadeToLevel("Options Menu"));
        
    }
    
    public void EnableOverwritePanel()
    {
        overwritePanel.SetActive(true);
        Button yesButton = GameObject.Find("Yes Button").GetComponent<Button>();
        yesButton.onClick.AddListener(() => GameManager.gameManager.CreateNewSaveFile());
    }
    public void DisableOverwritePanel()
    {
        overwritePanel.SetActive(false);
    }
    public void QuitGame()
    { 
        Application.Quit();
    }
}
