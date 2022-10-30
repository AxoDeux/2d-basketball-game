using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBallMM : MonoBehaviour {

    private Vector3 r = new Vector3(0, 0, -0.8f);
    void Start()    {
        //  LeanTween.rotate(gameObject, new Vector3(0, 0, 180), 5f).setLoopClamp();
    }

    private void Update() {
        transform.Rotate(r);
    }

}
