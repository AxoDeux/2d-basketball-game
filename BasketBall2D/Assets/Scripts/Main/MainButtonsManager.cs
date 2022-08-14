using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonsManager : MonoBehaviour
{
    private void Awake() {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay() {
        //show instructions
    }

    public void Customize() {
        SceneManager.LoadScene(2);
    }

    public void Options() {
        //Show Options
    }

    public void Quit() {
        Application.Quit();
    }


}
