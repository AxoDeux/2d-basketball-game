using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreSystem : MonoBehaviour
{
    private const string SCORES_FILE = "Scores";
    private int lives = 3;
    public int Lives { get { return lives; } }

    private int coins = 0;
    public int Coins { get { return coins; } }

    private int catcherAdded = 0;
    public int CatcherAdded { get { return CatcherAdded; } }


    [SerializeField]
    private TMP_Text timeText = null;

    [SerializeField]
    private TMP_Text livesText = null;

    [SerializeField]
    private TMP_Text coinsText = null;

    [SerializeField]
    private float timeLimit = 120;

    private float currTimeRemaining = 0;

    [SerializeField]
    private TMP_Text scoreText = null;

    [SerializeField]
    private TMP_Text highscoreText = null;

    [SerializeField]
    private TMP_Text timeScoreText = null;

    [SerializeField]
    private TMP_Text minTimeScoreText = null;

    [SerializeField]
    private GameObject scoreCard = null;

    private string timeString = string.Empty;
    string minutes = string.Empty;
    string seconds = string.Empty;

    private bool shouldPauseTime = false;

    private int loadedScore = 0;
    private int loadedTime = 0;

    Dictionary<GameObject, Vector2> coinsMap;

    private void Awake() {
        SaveSystem.Init();
        currTimeRemaining = timeLimit;
        coinsMap = new Dictionary<GameObject, Vector2>();
    }

    private void OnEnable() {
        BallMovement.PlayerDiedEvent += HandlePlayerDeath;
        CoinMove.OnCoinCollectedEvent += HandleCoinCollection;
        CoinMove.StoreCoinEvent += FillCoinMap;
        ButtonManager.CatcherSpawnedEvent += HandleCatcherSpawned;
        ButtonManager.UnlockCatcherEvent += SetPauseBool;
        ButtonManager.RetryEvent += Reset;
        Hoop.ScoredEvent += HasScored;
    }

    private void OnDisable() {
        BallMovement.PlayerDiedEvent -= HandlePlayerDeath;
        CoinMove.OnCoinCollectedEvent -= HandleCoinCollection;
        CoinMove.StoreCoinEvent -= FillCoinMap;
        ButtonManager.CatcherSpawnedEvent -= HandleCatcherSpawned;
        ButtonManager.UnlockCatcherEvent -= SetPauseBool;
        ButtonManager.RetryEvent -= Reset;
        Hoop.ScoredEvent -= HasScored;

    }

    private void Update() {
        if(shouldPauseTime) return;
        currTimeRemaining -= Time.deltaTime;

        CheckTimeSize(currTimeRemaining);

        timeString = $"{minutes} : {seconds}";
        timeText.text = timeString;

        if(currTimeRemaining <= 0) {
            Debug.Log("Time up!!!");
        }
    }

    private void CheckTimeSize(float time) {
        if((int)time / 60 < 10) {
            minutes = $"0{(int)time / 60}";
        } else {
            minutes = $"{(int)time / 60}";
        }

        if((int)time % 60 < 10) {
            seconds = $"0{(int)time % 60}";
        } else {
            seconds= $"{(int)time % 60}";
        }
    }
    
    private void HandlePlayerDeath() {
        lives--;
        livesText.text = lives.ToString();
    }

    private void FillCoinMap(GameObject coin, Vector2 pos) {
        coinsMap.Add(coin, pos);
    }

    private void HandleCoinCollection() {
        coins++;
        coinsText.text = coins.ToString();
    }

    private void HandleCatcherSpawned() {
        catcherAdded++;
    }

    private void SetPauseBool(bool status) {
        shouldPauseTime = status;
    }

    private void HasScored() {
        shouldPauseTime = true;
        LoadScore();
        int currScore = GetScore();
        float currTimeUsed = timeLimit - currTimeRemaining;
        //check scores and times
        if(loadedScore < currScore) {
            loadedScore = currScore;
        }
        if(loadedTime > (int)currTimeUsed) {
            loadedTime = (int)currTimeUsed;
        }
        SaveScore(loadedScore, loadedTime);

        //fill values
        scoreText.text = $"Score: {currScore}";
        CheckTimeSize(currTimeUsed);
        string timeStr = $"{minutes} : {seconds}";
        timeScoreText.text = timeStr;

        if(loadedScore == -1 || loadedTime == Int32.MaxValue) {
            highscoreText.text = "-";
            minTimeScoreText.text = "-";
        } else {
            highscoreText.text = $"{loadedScore}";
            CheckTimeSize(loadedTime);
            timeStr = $"{minutes} : {seconds}";
            minTimeScoreText.text = timeStr;
        }        

        scoreCard.SetActive(true);
    }


    private int GetScore() {
        int score = 0;

        //lives lost 
        int lscore = -(3 - lives) * 50;

        //coins
        int cscore = coins * 50;

        //catchers spawned
        int catscore = -(catcherAdded) * 30;

        score = lscore + cscore + catscore;

        return score;
    }

    private void SaveScore(int score, int time) {
        SaveScoreObject sso = new SaveScoreObject {
            highScore = score,
            minTime = time
        };

        string json = JsonUtility.ToJson(sso);
        SaveSystem.Save(json, SCORES_FILE);
    }

    private void LoadScore() {
        string saveString = SaveSystem.Load(SCORES_FILE);
        if(saveString != null) {
            SaveScoreObject sso = JsonUtility.FromJson<SaveScoreObject>(saveString);
            loadedScore = sso.highScore;
            loadedTime = sso.minTime;
        }else {
            loadedScore = -1;
            loadedTime = Int32.MaxValue;
        }

    }
    private class SaveScoreObject {
        public int highScore;
        public int minTime;
    }

    private void Reset() {
        currTimeRemaining = timeLimit;
        lives = 3;
        livesText.text = lives.ToString();
        coins = 0;
        coinsText.text = coins.ToString();
        catcherAdded = 0;
        scoreCard.SetActive(false);
        shouldPauseTime = false;

        foreach(KeyValuePair<GameObject, Vector2> pair  in coinsMap) {            
            pair.Key.transform.position = pair.Value;
            pair.Key.SetActive(true);
        }
    }
}
