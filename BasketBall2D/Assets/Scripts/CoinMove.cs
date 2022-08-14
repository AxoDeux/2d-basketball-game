using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    private const float MOVE_SPEED = 10f;
    private const float RADIUS = 4f;

    private bool isCaughtInMagnet = false;
    private GameObject ball = null;

    public delegate void OnCoinCollectedEventHandler();
    public static OnCoinCollectedEventHandler OnCoinCollectedEvent;

    private int layerMask;

    private void Awake() {
        ball = GameObject.FindGameObjectWithTag("Player");

        layerMask = LayerMask.NameToLayer("PlayerLayer");
    }

    void Update()
    {        
        if(isCaughtInMagnet && ball) {
            transform.position = Vector3.MoveTowards(transform.position, ball.transform.position, MOVE_SPEED * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Magnet")) {
            isCaughtInMagnet = true;
            Debug.Log("Caught in magnet");
        }

        if(collision.CompareTag("Player")) {
            Debug.Log("Coin Enterred collider, trigger");
            OnCoinCollectedEvent.Invoke();
            Destroy(gameObject, 0.05f);
        }
    }
}
