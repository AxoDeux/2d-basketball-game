using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ScoreSystem : MonoBehaviour
{
    private int lives = 3;
    public int Lives { get { return lives; } }

    private int coins = 0;
    public int Coins { get { return coins; } }

    [SerializeField]
    private TMP_Text timeText = null;

    [SerializeField]
    private TMP_Text livesText = null;

    [SerializeField]
    private TMP_Text coinsText = null;

    [SerializeField]
    private float timeLimit = 120;

    private string timeString = string.Empty;
    string minutes = string.Empty;
    string seconds = string.Empty;
    string milliseconds = string.Empty;
    StringBuilder msBuilder;

    private void OnEnable() {
        BallMovement.PlayerDiedEvent += HandlePlayerDeath;
        CoinMove.OnCoinCollectedEvent += HandleCoinCollection;
    }

    private void OnDisable() {
        BallMovement.PlayerDiedEvent -= HandlePlayerDeath;
        CoinMove.OnCoinCollectedEvent -= HandleCoinCollection;
    }

    private void Update() {

        timeLimit -= Time.deltaTime;

        CheckTimeSize();

        timeString = $"{minutes} : {seconds} : {milliseconds}";
        timeText.text = timeString;
        timeString = string.Empty;

        if(timeLimit <= 0) {
            Debug.Log("Time up!!!");
        }
    }

    private void CheckTimeSize() {
        if((int)timeLimit / 60 < 10) {
            minutes = $"0{(int)timeLimit / 60}";
        } else {
            minutes = $"{(int)timeLimit / 60}";
        }

        msBuilder = new StringBuilder((timeLimit % 60).ToString());

        if((int)timeLimit % 60 < 10) {
            seconds = $"0{(int)timeLimit % 60}";
            msBuilder.Remove(0, 2);                  //one number and one dot. ex: 4.3252 becomes 3252

        } else {
            seconds= $"{(int)timeLimit % 60}";
            msBuilder.Remove(0, 3);                  //two numbers and one dot
        }

        milliseconds = msBuilder.ToString();
        if(milliseconds.Length >= 3) {
            milliseconds = milliseconds.Substring(0, 2);
        }
    }
    
    private void HandlePlayerDeath() {
        lives--;
        livesText.text = lives.ToString();
    }

    private void HandleCoinCollection() {
        coins++;
        coinsText.text = coins.ToString();
    }


}
