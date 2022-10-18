using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBouncePlatform : MonoBehaviour
{
    [SerializeField] private int bounceLimit = 0;
    int bounceCount = 0;

    public delegate void OnLimitedBouncePlatformSpawn(GameObject platform, Vector2 pos);
    public static event OnLimitedBouncePlatformSpawn WeakPlatformSpawnedEvent;

    private void Start() {
        WeakPlatformSpawnedEvent.Invoke(gameObject, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Player")) { return; }

        bounceCount++;
        if(bounceCount >= bounceLimit) {
            SoundManager.PlaySound(SoundManager.Sounds.BallBounceHard);
            gameObject.SetActive(false);
        } else {
            SoundManager.PlaySound(SoundManager.Sounds.BallBounce);
        }
    }
}
