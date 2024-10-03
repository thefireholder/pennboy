using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /* should keep track of all scores. all scores will be sent here */
    [SerializeField]
    private int score;

    public void resetScore()
    {
        /* score may be negative, but must be integer */
        score = 0;
    }

    public void addScore(int num)
    {
        /* score may be negative, but must be integer */
        score += num;
    }

    public int getScore()
    {
        return score;
    }
}
