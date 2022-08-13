using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsButtonManager : MonoBehaviour
{
    public void Back() {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int levelBuildScene) {
        SceneManager.LoadScene(levelBuildScene);
    }
}
