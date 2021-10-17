using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : MonoBehaviour
{
    public AudioMixer audioMixer;
    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        var groups = audioMixer.FindMatchingGroups("Master");
        foreach (var a in groups) Debug.Log(a.name);
        for (int i = 0; i < 10; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = groups[2];
            audioSources.Add(audioSource);
        }
    }

    public void Play(AudioClip clip, bool loop = false)
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = clip;
                audioSource.loop = loop;
                audioSource.Play();
                return;
            }
        }
        Debug.LogWarning("Can't play the clip:the audioSourcePool is full");
    }
    public static void Play(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
        return;
    }
}
