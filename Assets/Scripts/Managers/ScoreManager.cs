using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    public void IncreaseScore(int score)
    {
        totalScore += score;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
