using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBall : MonoBehaviour
{
    [SerializeField]
    private GameObject magnetField = null;

    private Rigidbody2D rb = null;
    private BallMovement ballMovement = null;

    private const float ABILITY_DURATION = 15f;

    public bool isSecondLife = false;
    public bool isMagnetActive = false;

    private enum Abilities {
        LinearMove,
        SecondLife,
        Magnetic
    };

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        ballMovement = GetComponent<BallMovement>();
    }

    private void OnEnable() {
        ButtonManager.RetryEvent += Reset;
        Hoop.ScoredEvent += Reset;
    }
    private void OnDisable() {
        ButtonManager.RetryEvent -= Reset;
        Hoop.ScoredEvent -= Reset;
    }

    //Linear movement ball
    public void OnLinearMove() {
        rb.gravityScale = 0;
        StartCoroutine(AddAbilityDelay(Abilities.LinearMove));
    }

    //Second life Ball - Ball gets back to the previous pos on falling 
    public void ProvideSecondLife() {
        isSecondLife = true;
        StartCoroutine(AddAbilityDelay(Abilities.SecondLife));
    }

    //Magnetic Ability
    public void MagnetOn() {
        magnetField.SetActive(true);
        isMagnetActive = true;
        StartCoroutine(AddAbilityDelay(Abilities.Magnetic));
    }

    private IEnumerator AddAbilityDelay(Abilities ability) {
        yield return new WaitForSeconds(ABILITY_DURATION);

        switch(ability) {
            case Abilities.LinearMove:
                rb.gravityScale = 2;
                break;
            case Abilities.SecondLife:
                isSecondLife = false;
                break;
            case Abilities.Magnetic:
                magnetField.SetActive(false);
                isMagnetActive = false;
                break;
        }
    }

    private void Reset() {
        StopAllCoroutines();
        rb.gravityScale = 2;

        isSecondLife = false;

        magnetField.SetActive(false);
        isMagnetActive = false;
    }

}
