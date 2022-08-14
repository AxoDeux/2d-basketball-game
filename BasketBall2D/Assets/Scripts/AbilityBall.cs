using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBall : MonoBehaviour
{
    [SerializeField]
    private GameObject magnetField = null;

    private Rigidbody2D rb = null;
    private BallMovement ballMovement = null;

    private const float ABILITY_DURATION = 5f;

    public bool isSecondLife = false;
    public bool isMagnetActive = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        ballMovement = GetComponent<BallMovement>();
    }


    //Linear movement ball
    public void OnLinearMove() {
        rb.gravityScale = 0;
        StartCoroutine(AddAbilityDelay());
    }

    //Second life Ball - Ball gets back to the previous pos on falling 
    public void ProvideSecondLife() {
        //get the og pos and set it
        isSecondLife = true;

        //dont change scores

    }

    //Magnetic Ability
    public void MagnetOn() {
        magnetField.SetActive(true);
        isMagnetActive = true;
        StartCoroutine(AddAbilityDelay());
    }

    private IEnumerator AddAbilityDelay() {
        yield return new WaitForSeconds(ABILITY_DURATION);
        Reset();
    }

    private void Reset() {
        rb.gravityScale = 2;

        magnetField.SetActive(false);
        isMagnetActive = false;
    }

}
