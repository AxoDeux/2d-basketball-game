using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsHandler : MonoBehaviour
{
    private const float EVENT_DURATION = 5f;
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

    public delegate bool InverseGravityHandler(bool isGravityEvent);
    public static event InverseGravityHandler inverseGravityEvent;
    public static bool isGravityEvent = false;

    public enum Events {
        InverseGravity,
        RandomForceField
    };

    public void InvertGravity() {
        isGravityEvent = true;
        //inverseGravityEvent.Invoke(isGravityEvent);

        ball.GetComponent<Rigidbody2D>().gravityScale = -2;
        StartCoroutine(EventDurationDelay(Events.InverseGravity));

        topWall.tag = FLOOR_TAG;
        floor.tag = WALL_TAG;
    }

    public void ActivateForceField() {
        forceField.SetActive(true);
        forceField.GetComponent<AreaEffector2D>().forceAngle = Random.Range(0, 359);

        StartCoroutine(EventDurationDelay(Events.RandomForceField));
    }

    private IEnumerator EventDurationDelay(Events eventName) {
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


}

