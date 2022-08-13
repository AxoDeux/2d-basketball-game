using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBouncePlatform : MonoBehaviour
{
    [SerializeField] private int bounceLimit = 0;
    int bounceCount = 0;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Player")) { return; }

        bounceCount++;
        if(bounceCount >= bounceLimit) {
            gameObject.SetActive(false);
        }
    }
}
