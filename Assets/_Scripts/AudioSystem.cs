using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : Singleton<AudioSystem> {
    [SerializedDictionary("Music Name", "Audio File")]
    public SerializedDictionary<string,AudioClip> musicSounds;
    [SerializedDictionary("SFX Name", "Audio File")]
    public SerializedDictionary<string,AudioClip> sfxSounds;
    [SerializeField] private AudioSource _musicSource, _soundsSource;
    [SerializeField] public AudioMixer audioMixer;
    public bool AutoPlayMusicOnSceneChange = true;
    [SerializedDictionary("SceneName", "MusicNameToPlay")] 
    public SerializedDictionary<string,string> MusicScenePairings = new SerializedDictionary<string,string>();
    
    [System.Serializable] public class Sound
    {
        public string name;
        public AudioClip clip;
    }
    private void OnEnable() {
        SceneManager.activeSceneChanged += ChangeMusicOnSceneChange;
    }
    private void OnDisable() {
        SceneManager.activeSceneChanged -= ChangeMusicOnSceneChange;
    }

/// <summary>
/// Loops by default
/// </summary>
    public void PlayMusic(string name, bool loop = true)
    {
        if(!musicSounds.ContainsKey(name))
        {
            Debug.Log("Music Name: "+name+" Not Found on Audio System Component.");
            return;
        }
        else if (musicSounds[name] == _musicSource.clip && _musicSource.isPlaying)
        {
            return;
        }
        else
        {
            _musicSource.loop = loop;
            _musicSource.clip = musicSounds[name];
            _musicSource.Play();
        }
    }
/// <summary>
/// <param name="volume">ranges from 0 to 1. Scales larger than one might cause clipping.
/// </summary>

    public void StopMusic()
    {
        _musicSource.Stop();
    }
    public void PlaySFX(string name, float volume = 1f)
    {
        if(!sfxSounds.ContainsKey(name))
        {
            Debug.Log("SFX Name: "+name+" Not Found on Audio System Component.");
        }
        else
        {
            _soundsSource.PlayOneShot(sfxSounds[name], volume);
        }
    }
    

    public void Play3DSound(AudioClip clip, Vector3 pos, float vol = 1) {
        _soundsSource.transform.position = pos;
        _soundsSource.PlayOneShot(clip, vol);
    }

    private void ChangeMusicOnSceneChange(Scene current, Scene next)
    {
        if(!AutoPlayMusicOnSceneChange) return;

        string nextSceneName = next.name;
        if(MusicScenePairings.ContainsKey(nextSceneName))
            PlayMusic(MusicScenePairings[nextSceneName]);
        else
            print(nextSceneName + "was not found in Music Scene pairings.\n Make sure this name was added to the Audio system or if no music was intended to play for this scene, ignore this message.");
    }
}