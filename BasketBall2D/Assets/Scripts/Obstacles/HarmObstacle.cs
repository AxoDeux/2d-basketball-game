using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmObstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Player")) { return; }

        BallMovement ball = collision.gameObject.GetComponent<BallMovement>();
        if(ball) {
            ball.PlayerDied();
        }

        Debug.Log("Player destroyed");
    }
}
