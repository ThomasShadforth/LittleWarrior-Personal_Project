using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource _audioSourceMusic;
    public AudioSource _audioSourceSFX;
    public AudioClip[] _soundtracks;
    public AudioClip[] _soundEffects;
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach(AudioClip song in _soundtracks)
        {
            if (SceneManager.GetActiveScene().name.Contains(song.name))
            {
                _audioSourceMusic.Stop();
                _audioSourceMusic.clip = song;
                _audioSourceMusic.Play();
            }
        }
    }


    public void playSFX(string FXName)
    {
        foreach (AudioClip SFX in _soundEffects)
        {
            if(SFX.name == FXName)
            {
                _audioSourceSFX.Stop();
                _audioSourceSFX.clip = SFX;
                _audioSourceSFX.Play();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GamePause.gamePaused)
        {
            _audioSourceMusic.Pause();
        }
        else
        {
            _audioSourceMusic.UnPause();
        }
    }
}
