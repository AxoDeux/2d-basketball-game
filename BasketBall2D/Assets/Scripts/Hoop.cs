using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hoop : MonoBehaviour
{
    [SerializeField] private TMP_Text currentScore = null;
    [SerializeField] private TMP_Text highScore = null;
    [SerializeField] private AudioSource rimShotAudio = null;
    [SerializeField] private AudioSource pointScoredAudio = null;

    public int score { get; set; }
    private int highScoreInt = 0;

    private string easyModeKey = "HighScore_Easy";
    private string hardModeKey = "HighScore_Hard";

    public bool hasScored { get; set; }
    private bool isModeEasy = true;


    private void Start()
    {
        highScoreInt = PlayerPrefs.GetInt(easyModeKey);
        highScore.text = highScoreInt.ToString();
    }

    //To check if player scored point.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody.velocity.y < 0 && !GameEventsHandler.isGravityEvent 
            || other.attachedRigidbody.velocity.y > 0 && GameEventsHandler.isGravityEvent)         //check if the ball exits from below only. removes the possibility of scoring twice due to rim collisions
        {
            score++;
            currentScore.text = score.ToString();
            hasScored = true;
            CheckScore();
            pointScoredAudio.Play();
        }
    }

    //To check is player hits rim
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rimShotAudio.Play();
    }


    private void CheckScore()
    {
        if (score >= highScoreInt)
        {
            Debug.Log(score);
            highScoreInt = score;
            highScore.text = highScoreInt.ToString();

            if (isModeEasy)
            {
                PlayerPrefs.SetInt(easyModeKey, highScoreInt);

            }
            else
            {
                PlayerPrefs.SetInt(hardModeKey, highScoreInt);
            }

        }
    }
    
    public void SetScore()
    {
        score = 0;
        currentScore.text = score.ToString();
    }

    //Check the mode to get/set the highscore accordingly in CheckScore() method
    public void SetHighScoreMode(bool isEasyModeActive)
    {
        if (isEasyModeActive)
        {
            highScoreInt = PlayerPrefs.GetInt(easyModeKey);
            highScore.text = highScoreInt.ToString();
        }

        else
        {
            highScoreInt = PlayerPrefs.GetInt(hardModeKey);
            highScore.text = highScoreInt.ToString();
        }

        isModeEasy = isEasyModeActive;
    }
}
