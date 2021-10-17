using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    private Dictionary<string, AudioClip> _audioDic;
    private AudioSourcePool audioSourcePool;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        audioSourcePool = GetComponent<AudioSourcePool>();
        _audioDic = new Dictionary<string, AudioClip>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (var clip in clips)
        {
            _audioDic.Add(clip.name, clip);
        }
    }

    public void PlayAudio(string audioname)
    {
        if (_audioDic.ContainsKey(audioname))
        {
            audioSourcePool.Play(_audioDic[audioname]);
        }
        else Debug.LogError("can't find the audio name:  " + audioname);
    }

    public void PlayAudio(string audioname, Vector3 position)
    {
        if (_audioDic.ContainsKey(audioname))
        {
            audioSourcePool.Play(_audioDic[audioname]);
        }
        else Debug.LogError("can't find the audio name:  " + audioname);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
}


