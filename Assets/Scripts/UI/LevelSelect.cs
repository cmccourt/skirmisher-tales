using UnityEngine.UI;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public Button[] levels;
    public Button homeButton;
    public int currentLevel;
    public AudioClip backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        AudioManager.audioManager.PlayBackgroundMusic(backgroundMusic);
        GameManager.gameManager.totalLevels = levels.Length;
        currentLevel = GameManager.gameManager.GetCurrentLevel();
        for(int i = 0; i < levels.Length; i++)
        {
            int levelNum = i + 1;
            levels[i].onClick.AddListener(() => GameManager.gameManager.LoadLevelAsync("Level " + levelNum));
            if(i + 1 > currentLevel)
            {
                levels[i].interactable = false;
            }
        }
    }

    public void QuitToMainMenu()
    {
        GameManager.gameManager.FadeToLevel("Main Menu");
    }
}
