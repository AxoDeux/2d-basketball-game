using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private bool xMovement = true;

    [Tooltip("Distance from one end to the other of the desired movement range.")]
    [SerializeField]
    private float range = 0f;

    [SerializeField]
    private float tweenTime = 0f;

    private void Start() {
        if(xMovement) {
            LeanTween.moveLocalX(gameObject, range, tweenTime).setLoopPingPong();
        } else {
            LeanTween.moveLocalY(gameObject, range, tweenTime).setLoopPingPong();
        }
    }
}
