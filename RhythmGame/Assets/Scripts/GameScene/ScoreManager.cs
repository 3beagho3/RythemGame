using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//100만 점수가 최대치
public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    public float scoreUpSpeed;

    private int score;
    private float viewScore;

    public void ScoreUp(string judgementResult)
    {
        if (judgementResult.Equals("Perfect"))
        {
            score += 35;
        }
        else if (judgementResult.Equals("Good"))
        {
            score += 25;
        }
        else if (judgementResult.Equals("Normal"))
        {
            score += 15;
        }
        else if (judgementResult.Equals("Bad"))
        {
            score += 10;
        }
    }
    void Update()
    {
        viewScore = Mathf.MoveTowards(viewScore, score, scoreUpSpeed);
        scoreText.text = string.Format("{0:0000000}", viewScore);
    }
}
