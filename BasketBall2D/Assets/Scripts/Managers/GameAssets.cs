using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour { 

    private static GameAssets _instance;
    public static GameAssets Instance {
        get {
            if(_instance == null) _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _instance;
        }
    }

    public SoundAudioClip[] soundsList;

    [System.Serializable]
    public class SoundAudioClip {
        public SoundManager.Sounds sound;
        public AudioClip audioClip;
    }

}
