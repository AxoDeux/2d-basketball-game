using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BallMovement : MonoBehaviour {

    [SerializeField] private float force = 0.1f;
    //[SerializeField] private TMP_Text debugText = null;
    [SerializeField] public Transform arrowPivot = null;
    [SerializeField] private Hoop hoopScript = null;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private AudioSource bounceAudio = null;

    public delegate void PlayerDiedEventHandler();
    public event PlayerDiedEventHandler PlayerDiedEvent;

    public Vector3 LastStationaryBallPos;

    private Vector2 initialPos;
    private Vector2 finalPos;
    private Vector2 direction;
    private Vector2 currentPos;
    private Vector2 originPos;
    private Vector2 arrowDirection;

    private int spawnPosNum;
    private bool isLeft;

    private Rigidbody2D rb;
    private Camera mainCam;
    private AbilityBall ability;

    private bool isClickOnUI = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        ability = GetComponent<AbilityBall>();
    }

    private void Start()
    {
        rb.isKinematic = true;

        spawnPosNum = 0;
        transform.position = spawnPoints[0].position;
        isLeft = true;

        LastStationaryBallPos = transform.position;
    }

    private void Update()
    {
        if(rb.velocity.magnitude != 0) { return; }      //So player doesnt control ball when its in air

        #region touchinput
        if(Input.touchCount == 1) {

            if(Input.GetTouch(0).phase == TouchPhase.Began) {
                if(IsPointerOverUIWithIgnores(Input.GetTouch(0).position)) { return; }       //for checking if click was not on UI

                initialPos = Input.GetTouch(0).position;
                originPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                //debugText.text = "Player is touching";
            }

            if(Input.GetTouch(0).phase == TouchPhase.Ended && initialPos != Vector2.zero) {     //initial pos should be newly captured not used from previous data
                arrowPivot.gameObject.SetActive(false);
                arrowPivot.transform.localScale = new Vector3(1, 1, 1);
                arrowPivot.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

                if(IsPointerOverUIWithIgnores(Input.GetTouch(0).position)) { return; }       //for checking if click was not on UI

                rb.isKinematic = false;

                //debugText.text = "Player lifted finger";

                finalPos = Input.GetTouch(0).position;
                direction = finalPos - initialPos;
                rb.AddForce(-direction * force);

                initialPos = Vector2.zero;
            }

            if(Input.GetTouch(0).phase == TouchPhase.Moved && initialPos != Vector2.zero) {
                if(IsPointerOverUIWithIgnores(Input.GetTouch(0).position)) {                     //for checking if click was not on UI
                    arrowPivot.gameObject.SetActive(false);
                    arrowPivot.transform.localScale = new Vector3(1, 1, 1);
                    arrowPivot.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    return;
                }

                /*currentPos = Input.GetTouch(0).position;
                direction = currentPos - initialPos;
                arrowPivot.gameObject.SetActive(true);
                arrowPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, Vector2.Angle(new Vector2(1, 0), direction)));
                arrowPivot.localScale = new Vector3(1, direction.sqrMagnitude / 100000, 1);*/

                currentPos = mainCam.ScreenToWorldPoint(Input.GetTouch(0).position);
                arrowDirection = currentPos - originPos;
                arrowPivot.gameObject.SetActive(true);

                arrowPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, 180 + Vector2.SignedAngle(new Vector2(1, 0), arrowDirection)));

                arrowPivot.localScale = new Vector3(arrowDirection.sqrMagnitude / 2, 1, 1);
            } 
        }
        #endregion

        #region Mouse Inputs
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIWithIgnores(Input.mousePosition))
        {
            initialPos = Input.mousePosition;
            //originPos = arrowPivot.position;
            originPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            //debugText.text = "Player is touching";

        }

        if (Input.GetMouseButtonUp(0) && initialPos != Vector2.zero)                    //initial pos should be newly captured not used from previous data
        {
            arrowPivot.gameObject.SetActive(false);
            arrowPivot.transform.localScale = new Vector3(1, 1, 1);
            arrowPivot.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

            if(IsPointerOverUIWithIgnores(Input.mousePosition)) { return; }           //for checking if click was not on UI

            rb.isKinematic = false;

            //debugText.text = "Player lifted finger";

            finalPos = Input.mousePosition;
            direction = finalPos - initialPos;
            rb.AddForce(-direction * force);

            initialPos = Vector2.zero;

        }

        if (Input.GetMouseButton(0) && initialPos != Vector2.zero)
        {
            if(IsPointerOverUIWithIgnores(Input.mousePosition)) {                     //for checking if click was not on UI
                arrowPivot.gameObject.SetActive(false);
                arrowPivot.transform.localScale = new Vector3(1, 1, 1);
                arrowPivot.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                return;
            }

            currentPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            arrowDirection = currentPos - originPos;
            arrowPivot.gameObject.SetActive(true);

            arrowPivot.localRotation = Quaternion.Euler(new Vector3(0, 0,180+ Vector2.SignedAngle(new Vector2(1, 0), arrowDirection)));

            arrowPivot.localScale = new Vector3(arrowDirection.sqrMagnitude/2, 1, 1);
        }
        #endregion
    }

    private bool IsPointerOverUIWithIgnores(Vector3 pointerPos) {                           //true if on UI ignoring those with PointerClickThrough class
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pointerPos;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for(int i = 0; i < raycastResults.Count; i++) {
            if(raycastResults[i].gameObject.GetComponent<PointerClickThrough>() != null) {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string otherTag = other.gameObject.tag;
        if (otherTag == "Floor")
        {
            if(!ability.isSecondLife) {
                //Set Spawn Point
                transform.position = spawnPoints[0].position;
            }            

            //if player has not scored then score resets
            if (!hoopScript.hasScored){
                hoopScript.SetScore();
            }

            hoopScript.hasScored = false;

            bounceAudio.Play();

            PlayerDied();
        }

        if (otherTag == "Walls")
        {
            bounceAudio.Play();
        }
    }

    public void ResetPlayerMovement() {
        //reseting player movement variables
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        arrowPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void PlayerDied() {
        //reseting player movement variables
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        arrowPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //player has actually died
        if(!ability.isSecondLife) {
            LastStationaryBallPos = spawnPoints[0].position;
            transform.position = LastStationaryBallPos;
        }

        PlayerDiedEvent.Invoke();
    }


}
