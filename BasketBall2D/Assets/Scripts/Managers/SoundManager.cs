using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager{
    
    public enum Sounds {
        BallBounce,
        BallBounceHard,
        BallCatch,
        BallHitNet,
        BallRimShot,
        WarningBell,
        Buzzer,
        SpikeImpact,
        LifeLost,
        GameOver,
        MagnetSuck,
        CoinCollected,
        LevelCompleted
    }
    public static void PlaySound(Sounds sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sounds sound) {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundsList) {
            if(soundAudioClip.sound == sound) return soundAudioClip.audioClip;
        }

        Debug.LogError("AudioClip " + sound + " Not Found");
        return null;
    }
    
}
