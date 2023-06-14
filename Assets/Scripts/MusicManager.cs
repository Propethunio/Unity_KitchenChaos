using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour {

    private const string playerPrefsMusicVolume = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = .4f;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(playerPrefsMusicVolume, volume);
        audioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if(volume > 1f) {
            volume = 0f;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(playerPrefsMusicVolume, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}