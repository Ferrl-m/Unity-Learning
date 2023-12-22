using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_PREFS_MUSIC = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    
    private float volume;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC, .3f);
        audioSource.volume = volume;
        Instance = this;
    }

    public void ChangeVolume() {
        volume += .1f;

        if(volume > 1f) {
            volume = 0f;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC, volume);
    }

    public float GetVolume() {
        return volume;
    }
}
