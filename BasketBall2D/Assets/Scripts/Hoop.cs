using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hoop : MonoBehaviour
{
    [SerializeField] private AudioSource rimShotAudio = null;
    [SerializeField] private AudioSource pointScoredAudio = null;

    public int score { get; set; }

    public bool hasScored { get; set; }


    //To check if player scored point.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody.velocity.y < 0 && !GameEventsHandler.isGravityEvent 
            || other.attachedRigidbody.velocity.y > 0 && GameEventsHandler.isGravityEvent)         //check if the ball exits from below only. removes the possibility of scoring twice due to rim collisions
        {

            hasScored = true;

            pointScoredAudio.Play();
        }
    }

    //To check is player hits rim
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rimShotAudio.Play();
    }
    
    public void SetScore()
    {

    }

}
