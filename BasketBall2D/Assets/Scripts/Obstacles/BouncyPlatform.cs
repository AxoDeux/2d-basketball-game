using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if(!collision.gameObject.CompareTag("Player")) { return; }

        SoundManager.PlaySound(SoundManager.Sounds.BallBounceHard);
    }
}
