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
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject buttonMask = null;        //spawn on buttons which have timer/cant be interacted.

    public delegate void RetryEventHandler();
    public static event RetryEventHandler RetryEvent;

    private const float UNLOCK_TIME = 15f;
    public delegate void UnlockCatcherEventHandler(bool unlockStatus);
    public static event UnlockCatcherEventHandler UnlockCatcherEvent;
    public bool isLocked = true;

    public delegate void CatcherSpawnedEventHandler(GameObject catcher);
    public static event CatcherSpawnedEventHandler CatcherSpawnedEvent;
    List<GameObject> newCatchers;

    private Dictionary<Image, float> imageToFillMap;
    List<Image> imgs;
    List<float> fills;
    List<Button> buttons;           //un interactable buttons

    private void Awake(){
        Screen.orientation = ScreenOrientation.Landscape;
        newCatchers = new List<GameObject>();

        imageToFillMap = new Dictionary<Image, float>();
        imgs = new List<Image>();
        fills = new List<float>();
        buttons = new List<Button>();
    }

    private void Update() {
        float delta = Time.deltaTime;

        if(imageToFillMap.Count > 0) {
            foreach(var pair in imageToFillMap) {
                if(pair.Value < 0) continue;            //removed from map & deleted in coroutine

                pair.Key.fillAmount = pair.Value - delta / UNLOCK_TIME;
                imgs.Add(pair.Key);
                fills.Add(pair.Key.fillAmount);
            }

            if(imgs.Count > 0) {
                for(int i = 0; i < imgs.Count; i++) {
                    imageToFillMap[imgs[i]] = fills[i];
                }
            }
            imgs.Clear();
            fills.Clear();
        }
    }

    #region standard game buttons
    public void OnPressQuit() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnOptions() {
        optionsPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnResume() {
        optionsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Retry() {
        RetryEvent.Invoke();
        StopAllCoroutines();        //stop any ongoing coroutines;

        foreach(GameObject obj in newCatchers) {
            Destroy(obj);
        }
        newCatchers.Clear();

        foreach(KeyValuePair<Image, float> pair in imageToFillMap) {
            Destroy(pair.Key.gameObject);
        }
        imageToFillMap.Clear();     //check if deleted

        foreach(Button button in buttons) {     // if coroutines are abruptly stopped.
            button.interactable = true;
        }
        buttons.Clear();
    }
    #endregion

    public void AbilityTimer(Button button) {
        GameObject obj = Instantiate(buttonMask, button.transform.position, Quaternion.identity, button.transform);
        imageToFillMap.Add(obj.GetComponent<Image>(), 1f);
        StartCoroutine(DisableInteraction(button, obj));
    }
    private IEnumerator DisableInteraction(Button button, GameObject mask) {
        button.interactable = false;
        buttons.Add(button);
        yield return new WaitForSeconds(UNLOCK_TIME);
        button.interactable = true;
        buttons.Remove(button);

        yield return new WaitForSeconds(1f);            //destroy the spawned mask
        imageToFillMap.Remove(mask.GetComponent<Image>());
        Destroy(mask);
    }

    public void SpawnCatcher() {
        Vector2 screenPosInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        Debug.Log(screenPosInWorld);
        GameObject obj = Instantiate(ballCatcherPrefab, screenPosInWorld, Quaternion.identity, null);

        CatcherSpawnedEvent.Invoke(obj);
    }

    public void OnUnlockCatcher(Button button) {
        isLocked = false;
        UnlockCatcherEvent.Invoke(true);        

        GameObject obj = Instantiate(buttonMask, button.transform.position, Quaternion.identity, button.transform);
        imageToFillMap.Add(obj.GetComponent<Image>(), 1f);

        StartCoroutine(StartUnlockTimer(button, obj));
    }

    private IEnumerator StartUnlockTimer(Button button, GameObject mask) {
        button.interactable = false;
        buttons.Add(button);
        yield return new WaitForSeconds(UNLOCK_TIME);
        button.interactable = true;
        buttons.Remove(button);
        isLocked = true;
        UnlockCatcherEvent.Invoke(false);

        yield return new WaitForSeconds(1f);
        imageToFillMap.Remove(mask.GetComponent<Image>());
        Destroy(mask);
    }

}
