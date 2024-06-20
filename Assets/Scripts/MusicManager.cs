using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
   public static MusicManager Instance { get; private set; }
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private AudioSource audiosSource;
    private float volume=1f;
    private void Awake() {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        audiosSource=GetComponent<AudioSource>();
        audiosSource.volume = volume;
    }
    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1.1f) {
            volume = 0;
        }
        audiosSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}
