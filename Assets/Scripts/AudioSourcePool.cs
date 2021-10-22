using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool 
{
    
    private List<AudioSource> audioSources = new List<AudioSource>();

    public AudioSourcePool(GameObject gameObject,AudioMixerGroup group)
    {
        for (int i = 0; i < 10; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = group;
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
