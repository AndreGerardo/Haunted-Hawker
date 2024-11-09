using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    Music,
    SoundFX
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private Sound[] _soundData;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }

        foreach (var sound in _soundData)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            sound.source = audioSource;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }


        SoundEvent.OnSoundPlayOnce += PlaySoundOnce;
        SoundEvent.OnBGMPlay += PlaySound;
        SoundEvent.OnStopPlay += StopSound;
    }

    private void OnDestroy()
    {
        SoundEvent.OnSoundPlayOnce -= PlaySoundOnce;
        SoundEvent.OnBGMPlay -= PlaySound;
        SoundEvent.OnStopPlay -= StopSound;
    }


    public void PlaySoundOnce(int ID)
    {

        if (ID < 0) return;
        Sound s = Array.Find(_soundData, Sound => Sound.audioID == ID);
        if (s == null)
        {
            Debug.LogError("Sound " + ID + "Not Found");
        }
        else
        {
            _soundData[ID].source.PlayOneShot(_soundData[ID].clip);
        }
    }

    public void PlaySound(int ID)
    {

        if (ID < 0) return;
        Sound s = Array.Find(_soundData, Sound => Sound.audioID == ID);
        if (s == null)
        {
            Debug.LogError("Sound " + ID + "Not Found");
        }
        else
        {
            _soundData[ID].source.Play();

        }

    }


    public void StopSound(int ID)
    {
        if (ID < 0) return;
        Sound s = Array.Find(_soundData, Sound => Sound.audioID == ID);
        if (s == null)
        {
            Debug.LogError("Sound " + ID + "Not Found");
        }
        else
        {

            _soundData[ID].source.Stop();

        }
    }
}


public static class SoundEvent
{
    public static Action<int> OnSoundPlayOnce;
    public static Action<int> OnBGMPlay;
    public static Action<int> OnStopPlay;

}

[System.Serializable]
public class Sound
{

    public string audioName;
    public int audioID;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    public bool loop;
    [HideInInspector] public AudioSource source;
}