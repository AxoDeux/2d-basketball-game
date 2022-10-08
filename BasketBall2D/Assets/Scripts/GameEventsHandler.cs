using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEventsHandler : MonoBehaviour
{
    private const float EVENT_DURATION = 10f;
    private const float COUNTDOWN = 4f;
    private const string WALL_TAG = "Walls";
    private const string FLOOR_TAG = "Floor";


    [SerializeField]
    private GameObject ball = null;

    [SerializeField]
    private GameObject topWall = null;

    [SerializeField]
    private GameObject floor = null;

    [SerializeField]
    private GameObject forceField = null;

    [SerializeField]
    private Image timeIndicator = null;

    [SerializeField]
    private TMP_Text eventNotice = null;

    public delegate bool InverseGravityHandler(bool isGravityEvent);
    public static event InverseGravityHandler inverseGravityEvent;
    public static bool isGravityEvent = false;

    private float time = -1;
    private float eventDuration = 0;

    private float countDown = 0;
    private bool isCountDown = false;

    public enum Events {
        InverseGravity,
        RandomForceField
    };

    private void Update() {
        float delta = Time.deltaTime;
        if(time > 0) {
            timeIndicator.fillAmount = (time / eventDuration);
            time -= delta;   
            if(time < 0) {
                timeIndicator.gameObject.SetActive(false);
            }
        }

        if(isCountDown) {
            
            countDown -= delta;
            int num = (int)countDown;
            if(num < 0) {
                isCountDown = false;
                eventNotice.gameObject.SetActive(false);
            } else {
                eventNotice.text = "Event starts in.... " + num.ToString();
            }
        }

    }

    public void InvertGravity() {
        isGravityEvent = true;


        StartCoroutine(CountdownToEvent(Events.InverseGravity));        
    }

    public void ActivateForceField() {
        StartCoroutine(CountdownToEvent(Events.RandomForceField));
        
    }
    private IEnumerator CountdownToEvent(Events eventName) {
        countDown = COUNTDOWN;
        eventNotice.gameObject.SetActive(true);
        isCountDown = true;

        yield return new WaitForSeconds(4f);

        switch(eventName) {
            case Events.InverseGravity:
                ball.GetComponent<Rigidbody2D>().gravityScale = -2;
                topWall.tag = FLOOR_TAG;
                floor.tag = WALL_TAG;

                StartIndicator(EVENT_DURATION);
                StartCoroutine(EventDurationDelay(Events.InverseGravity));
                break;

            case Events.RandomForceField:
                forceField.SetActive(true);
                forceField.GetComponent<AreaEffector2D>().forceAngle = Random.Range(0, 359);

                StartIndicator(EVENT_DURATION);
                StartCoroutine(EventDurationDelay(Events.RandomForceField));
                break;
        }
    }

    private IEnumerator EventDurationDelay(Events eventName) {              //reset values
        yield return new WaitForSeconds(EVENT_DURATION);

        switch(eventName) {
            case Events.InverseGravity:
                ball.GetComponent<Rigidbody2D>().gravityScale = 2;
                topWall.tag = WALL_TAG;
                floor.tag = FLOOR_TAG;
                isGravityEvent = false;
                //inverseGravityEvent.Invoke(isGravityEvent);
                break;

            case Events.RandomForceField:
                forceField.SetActive(false);
                break;
        }

    }



    private void StartIndicator(float duration) {
        timeIndicator.fillAmount = 1;
        timeIndicator.gameObject.SetActive(true);
        time = duration;
        eventDuration = duration;
    }


}

