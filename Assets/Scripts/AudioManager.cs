using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    [Range(-15,15)]
    public float BGMVolumn;
    [Range(-15, 15)]
    public float SFXVolumn;
    [Range(-15, 15)]
    public float MasterVolumn;

    private Dictionary<string, AudioClip> _audioDic;
    private AudioSourcePool SFXAudioPool;
    private AudioSource BGMSudioSource;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        AudioMixerGroup[] groups = audioMixer.FindMatchingGroups("Master");
        foreach (var a in groups) Debug.Log(a.name);

        SFXAudioPool = new AudioSourcePool(gameObject, groups[2]);

        BGMSudioSource = gameObject.AddComponent<AudioSource>();
        BGMSudioSource.outputAudioMixerGroup =groups[1];

        _audioDic = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (var clip in clips)
        {
            _audioDic.Add(clip.name, clip);
        }

    }

    public void PlaySFXAudio(string audioname)
    {
        if (_audioDic.ContainsKey(audioname))
        {
            SFXAudioPool.Play(_audioDic[audioname]);
        }
        else Debug.LogError("can't find the SFXaudio name:  " + audioname);
    }

    public void PlayBGMAudio(string audioname)
    {
        if (_audioDic.ContainsKey(audioname))
        {
            BGMSudioSource.loop = true;
            BGMSudioSource.clip = _audioDic[audioname];
            BGMSudioSource.Play();
        }
        else Debug.LogError("can't find the BGMaudio name:  " + audioname);
    }

    #region only uesd for ui
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        MasterVolumn = volume;
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        SFXVolumn = volume;
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
        BGMVolumn = volume;
    }
    #endregion
}


