using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeEffect : MonoBehaviour
{
    private Animator fadeAnimator;
    private string levelName;
    private void Awake()
    {
        fadeAnimator = GetComponent<Animator>();
    }
    public void EnableFadeToLevel(string sceneName)
    {
        levelName = sceneName;
        fadeAnimator.SetTrigger("fade_out");
    }
    public void LoadLevel()
    {
        GameManager.gameManager.LoadLevel(levelName);
        fadeAnimator.SetTrigger("fade_out");
    }
}
