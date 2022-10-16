using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    private const float MOVE_SPEED = 10f;
    private const float RADIUS = 4f;

    private bool isCaughtInMagnet = false;
    private GameObject ball = null;

    public delegate void StoreCoinEventHandler(GameObject coin, Vector2 pos);
    public static StoreCoinEventHandler StoreCoinEvent;

    public delegate void OnCoinCollectedEventHandler();
    public static OnCoinCollectedEventHandler OnCoinCollectedEvent;

    private int layerMask;

    private void Awake() {
        ball = GameObject.FindGameObjectWithTag("Player");

        layerMask = LayerMask.NameToLayer("PlayerLayer");
    }

    private void Start() {
        StoreCoinEvent.Invoke(gameObject, transform.position);
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
        }

        if(collision.CompareTag("Player")) {
            isCaughtInMagnet = false;
            OnCoinCollectedEvent.Invoke();
            StartCoroutine(CoinCollectionDelay());
        }
    }

    private IEnumerator CoinCollectionDelay() {
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
}
