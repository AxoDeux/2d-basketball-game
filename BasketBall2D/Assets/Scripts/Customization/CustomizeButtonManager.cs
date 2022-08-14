using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizeButtonManager : MonoBehaviour
{
    private void Awake() {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    public void Back() {
        SceneManager.LoadScene(0);
    }
}
