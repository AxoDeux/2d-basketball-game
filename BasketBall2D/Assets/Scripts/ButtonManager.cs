using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Transform hoopObject = null;
    [SerializeField] private Hoop hoopScript = null;
    [SerializeField] private GameObject ballCatcherPrefab = null;

    private const float UNLOCK_TIME = 15f;
    public delegate void UnlockCatcherEventHandler(bool unlockStatus);
    public event UnlockCatcherEventHandler UnlockCatcherEvent;
    private bool isLocked = true;

    private Vector3 hoopObjPos;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        hoopObjPos = hoopObject.position;
    }

    public void EasyMode()
    {
        LeanTween.reset();
        hoopObject.position = hoopObjPos;
        hoopScript.SetHighScoreMode(true);
    }
    
    public void HardMode()
    {
        LeanTween.moveLocalY(hoopObject.gameObject, hoopObject.position.y - 2, 5f).setEaseInOutCubic().setLoopPingPong();       //moves hoop vertically
        hoopScript.SetHighScoreMode(false);
    }

    public void SpawnCatcher() {
        Vector2 screenPosInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
        Debug.Log(screenPosInWorld);
        Instantiate(ballCatcherPrefab, screenPosInWorld, Quaternion.identity, null);
    }

    public void OnPressQuit()
    {
        Application.Quit();
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
