using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public PlayModeSO playModeSO;
    public AudioClip[] audioClips;
    public List<AudioDataStruct> list = new List<AudioDataStruct>();
    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        instance = gameObject.GetComponent<AudioManager>();
    }

    private void Update()
    {
        SetPitchByTimeScale();
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
        AudioDataStruct data = new AudioDataStruct
        {
            clipName = clipName,
            source = sourceObj.name
        };

        list.Add(data);
    }

    private void SetPitchByTimeScale()
    {
        float pitchScale = Time.timeScale;
        float defaltLerpSpeed = 0.05f;
        float fastLerpSpeed = 0.5f;

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i] != null && audioSources[i].volume == pitchScale) return;
            if (audioSources[i] == null) continue;

            float lerpProgress = audioSources[i].volume > pitchScale ? defaltLerpSpeed : fastLerpSpeed;
            pitchScale = Mathf.Lerp(audioSources[i].volume, pitchScale, lerpProgress);

            audioSources[i].pitch = pitchScale;
            audioSources[i].volume = pitchScale;
        }
    }

    public void PlaySound(string clipName, GameObject sourceObj)
    {
        if (sourceObj == null) return;

        if (!playModeSO.isReplayMode)
        {
            SaveAudioData(clipName, sourceObj);
        }
        if (!sourceObj.GetComponent<AudioSource>())
        {
            sourceObj.AddComponent<AudioSource>();
        }
        sourceObj.GetComponent<AudioSource>().PlayOneShot(FindAudioClip(clipName));
        audioSources.Add(sourceObj.GetComponent<AudioSource>());
    }
}
