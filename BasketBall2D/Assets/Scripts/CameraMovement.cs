using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    private Transform basketBall = null;

    [SerializeField]
    private float CameraMotionTime = 5f;
    private Camera cam;

    private Vector2 originalPos;
    private Vector2 finalPos;

    private Vector3 offset;
    private Vector3 camPos;

    private void Awake() {
        cam = GetComponent<Camera>();
        originalPos = transform.position;
        finalPos = basketBall.position;
        offset = new Vector3(0, 0, -basketBall.position.z - 10);
    }

    private void Start() {
        cam.orthographicSize = 4.5f;
        LeanTween.move(gameObject, finalPos, CameraMotionTime);        
    }
    private void FixedUpdate() {
        camPos = basketBall.position + offset;
        transform.position = Vector3.Lerp(transform.position, camPos, 10f * Time.fixedDeltaTime);
    }

}
