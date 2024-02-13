using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level;
    public int totalLevels;
    public GameObject loadingScreen;
    public Slider loadSlider;
    public Text loadPercentText;
    public static GameManager gameManager;
    public bool frozenUnlocked;
    public bool heavyUnlocked;
    public PlayerData playerData;
    private string levelName;

    private void Awake()
    {
        if (gameManager == null) gameManager = this;
        else if (gameManager != this) Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
    }

    public PlayerData LoadSaveFile()
    {
        playerData = SaveSystem.LoadPlayer();
        Debug.Log(playerData);
        return playerData;
    }

    public void SaveFile()
    {
        SaveSystem.SavePlayer(this);
    }

    public void CreateNewSaveFile()
    {
        level = 1;
        heavyUnlocked = false;
        frozenUnlocked = false;
        SaveSystem.CreateNewFile(this);
        FadeToLevel("Level Select");
    }

    public void LoadLevelAsync(string sceneName)
    {
        levelName = sceneName;
        StartCoroutine(LoadAsynchronously(levelName));      
    }
    public void FadeToLevel(string sceneName)
    {
        levelName = sceneName;
        FadeEffect fadeEffect = FindObjectOfType<FadeEffect>();
        fadeEffect.EnableFadeToLevel(levelName);
    }
    public void LoadLevel(string sceneName)
    {
        levelName = sceneName;
        SceneManager.LoadScene(levelName);
    }
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadSlider.value = progress;
            loadPercentText.text = progress * 100f + "%";
            yield return null;
        }
        loadingScreen.SetActive(false);
    }

    public int GetCurrentLevel()
    {
        playerData = LoadSaveFile();
        if(playerData == null)
        {
            level = 1;
        }
        else
        {
            level = playerData.level;
        }
        Debug.Log(playerData);
        return level;
    }

    public void LevelCompleted()
    {
        level += 1;
        SaveSystem.SavePlayer(this);
        if(level == totalLevels)
        {
            Debug.Log("Game Completed");
            LoadLevelAsync("Level Select");
        }
        else
        {
            LoadLevelAsync("Level Select");
        }
    }
}
