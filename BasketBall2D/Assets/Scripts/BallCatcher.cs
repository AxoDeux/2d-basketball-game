using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallCatcher : MonoBehaviour, IDragHandler, IDropHandler {

    private const float CATCH_TIME = 0.75f;
    [SerializeField]
    private Transform catcherPos = null;

    [SerializeField]
    private Canvas canvas = null;

    private float distance = 0f;
    private bool isBallCaught = false;

    private bool isUnlocked = false;
    private Vector3 lastMousePos = Vector3.zero;
    private Camera mainCam;

    private ButtonManager buttonManager;

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach(Canvas item in canvases) {
            if(item.name == "UICanvas") {
                canvas = item;
                break;
            }
        }

        buttonManager = FindObjectOfType<ButtonManager>();
        mainCam = Camera.main;
    }

    private void OnEnable() {
        buttonManager.UnlockCatcherEvent += UnlockPositions;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //check the object is ball
        if(!collision.CompareTag("Player")) { return; }
        if(isBallCaught) { return; }
        isBallCaught = true;


        BallMovement ball = collision.GetComponent<BallMovement>();
        ball.ResetPlayerMovement();
        SetBallPosition(collision.gameObject);
        if(ball) {
            ball.LastStationaryBallPos = transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(!collision.CompareTag("Player")) { return; }
        isBallCaught = false;
    }

    private void SetBallPosition(GameObject ballObject) {
        LeanTween.move(ballObject, catcherPos.position, CATCH_TIME).setEaseInQuad();
    }


    public void OnDrag(PointerEventData eventData) {
        if(!isUnlocked) { return; }

        if(Input.touchCount == 1) {
            if(lastMousePos != Vector3.zero) {
                //Check if works on phone or use ScreenToWorldPoint
                Vector2 touchDelta = (mainCam.ScreenToWorldPoint(Input.GetTouch(0).position) - lastMousePos) / canvas.scaleFactor;
                transform.localPosition += new Vector3(touchDelta.x, touchDelta.y, 0);
            }

            lastMousePos = mainCam.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        

        if(Input.GetMouseButton(0)) {
            if(lastMousePos != Vector3.zero) {
                Vector2 touchDelta = (mainCam.ScreenToWorldPoint(Input.mousePosition) - lastMousePos) / canvas.scaleFactor;
                transform.localPosition += new Vector3(touchDelta.x, touchDelta.y, 0);
            }

            lastMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void UnlockPositions(bool status) {
        isUnlocked = status;
    }
}
