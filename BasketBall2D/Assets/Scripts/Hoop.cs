using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hoop : MonoBehaviour
{
    [SerializeField] private AudioSource rimShotAudio = null;
    [SerializeField] private AudioSource pointScoredAudio = null;

    public delegate void ScoredEventHandler();
    public static event ScoredEventHandler ScoredEvent;

    public int score { get; set; }


    //To check if player scored point.
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if (other.attachedRigidbody.velocity.y < 0 && !GameEventsHandler.isGravityEvent 
            || other.attachedRigidbody.velocity.y > 0 && GameEventsHandler.isGravityEvent)         //check if the ball exits from below only. removes the possibility of scoring twice due to rim collisions
        {
            ScoredEvent.Invoke();
            pointScoredAudio.Play();
        }
    }

    //To check is player hits rim
    private void OnCollisionEnter2D(Collision2D collision) {
        rimShotAudio.Play();
    }

}
