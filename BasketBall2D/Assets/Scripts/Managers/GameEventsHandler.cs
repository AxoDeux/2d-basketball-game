using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEventsHandler : MonoBehaviour
{
    private const float EVENT_GAP = 20f;
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

    [SerializeField]
    private List<ParticleSystemForceField> forceFields = new List<ParticleSystemForceField>();

    public delegate bool InverseGravityHandler(bool isGravityEvent);
    public static event InverseGravityHandler inverseGravityEvent;
    public static bool isGravityEvent = false;

    private float time = -1;
    private float eventDuration = 0;

    private float countDown = 0;
    private bool isCountDown = false;

    private float timer = EVENT_GAP;
    private bool eventOngoing = false;
    public enum Events {
        InverseGravity,
        RandomForceField
    };

    private void OnEnable() {
        ButtonManager.RetryEvent += Reset;
    }
    private void OnDisable() {
        ButtonManager.RetryEvent -= Reset;
    }

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

        if(eventOngoing) return;            //add check for game started.

        timer -= delta;
        if(timer < 0) {
            eventOngoing = true;
            int randInt = Random.Range(0, 2);
            switch(randInt) {
                case 0:
                    InvertGravity();
                    break;
                case 1:
                    ActivateForceField();
                    break;
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
                SetForceField(new Vector2(0, 1));

                StartIndicator(EVENT_DURATION);
                StartCoroutine(EventDurationDelay(Events.InverseGravity));
                break;

            case Events.RandomForceField:
                forceField.SetActive(true);
                int angle = Random.Range(0, 359);
                forceField.GetComponent<AreaEffector2D>().forceAngle = angle;
                float rad = (float)Mathf.Deg2Rad*angle;
                Vector2 direction = new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad));
                SetForceField(direction);
                Debug.Log(direction + " " + angle);

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
                SetForceField(new Vector2(0, -1));
                break;

            case Events.RandomForceField:
                forceField.SetActive(false);
                SetForceField(new Vector2(0, -1));
                break;
        }

        eventOngoing = false;
        timer = EVENT_GAP;
    }

    private void StartIndicator(float duration) {
        timeIndicator.fillAmount = 1;
        timeIndicator.gameObject.SetActive(true);
        time = duration;
        eventDuration = duration;
    }

    private void Reset() {
        StopAllCoroutines();

        time = -1;
        isCountDown = false;
        eventDuration = 0;
        countDown = 0;
        eventNotice.gameObject.SetActive(false);
        timeIndicator.gameObject.SetActive(false);

        isGravityEvent = false;
        ball.GetComponent<Rigidbody2D>().gravityScale = 2;
        topWall.tag = WALL_TAG;
        floor.tag = FLOOR_TAG;
        forceField.SetActive(false);

        eventOngoing = false;
        timer = EVENT_GAP;
    }
    private void SetForceField(Vector2 direction) {
        direction *= 10;
        foreach(ParticleSystemForceField forceField in forceFields) {
            forceField.directionX = direction.x;
            forceField.directionY = direction.y;
        }        
    }
}

