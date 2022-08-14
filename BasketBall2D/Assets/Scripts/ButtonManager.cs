using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Transform hoopObject = null;
    [SerializeField] private GameObject ballCatcherPrefab = null;

    private const float UNLOCK_TIME = 15f;
    public delegate void UnlockCatcherEventHandler(bool unlockStatus);
    public event UnlockCatcherEventHandler UnlockCatcherEvent;
    private bool isLocked = true;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }


    public void SpawnCatcher() {
        Vector2 screenPosInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
        Debug.Log(screenPosInWorld);
        Instantiate(ballCatcherPrefab, screenPosInWorld, Quaternion.identity, null);
    }

    public void OnPressQuit()
    {
        SceneManager.LoadScene(0);
    }

    public void OnUnlockCatcher(Button button) {
        isLocked = false;
        UnlockCatcherEvent.Invoke(true);
        StartCoroutine(StartUnlockTimer(button));
    }

    private IEnumerator StartUnlockTimer(Button button) {
        button.interactable = false;

        yield return new WaitForSeconds(UNLOCK_TIME);
        button.interactable = true;
        isLocked = true;
        UnlockCatcherEvent.Invoke(false);
    }


}
