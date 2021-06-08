using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public PlayModeSO playModeSO;
    public AudioClip[] audioClips;

    private void Awake()
    {
        instance = gameObject.GetComponent<AudioManager>();
    }

    private AudioClip FindAudioClip(string clipName)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name.Equals(clipName))
            {
                return clip;
            }
        }

        return audioClips[0];
    }

    private void SaveAudioData(string clipName, GameObject sourceObj)
    {

    }

    public void PlaySound(string clipName, GameObject sourceObj)
    {
        if (!playModeSO.isReplayMode)
        {
            SaveAudioData(clipName, sourceObj);
        }
        if (!sourceObj.GetComponent<AudioSource>())
        {
            sourceObj.AddComponent<AudioSource>();
        }
        sourceObj.GetComponent<AudioSource>().PlayOneShot(FindAudioClip(clipName));
    }
}
