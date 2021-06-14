using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OperateManager : MonoBehaviour
{
    public GameObject title;
    public GameObject start;
    public AudioClip boot;

    private float startTime = 0;
    private bool loopStarted = false;

    private void Start()
    {
        StartCoroutine(SetOperate());
        PlayOneShot(boot, 1f);
    }

    private void Update()
    {
        if (!IsPlaying() && !loopStarted)
        {
            gameObject.GetComponent<AudioSource>().Play();
            loopStarted = true;
        }
    }

    IEnumerator SetOperate()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);

            TypingOperate typing = gameObject.transform.GetChild(i).GetComponent<TypingOperate>();
            float length = typing.text.Length + 2;
            float delay = typing.delay;

            yield return new WaitForSeconds(length * delay);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        title.SetActive(true);
        start.SetActive(true);

        while (true)
        {
            GameObject buttonText =  start.transform.GetChild(0).gameObject;
            float delay = !title.activeSelf ? 1f : 0.5f;

            buttonText.SetActive(!buttonText.activeSelf);

            yield return new WaitForSeconds(delay);
        }
    }

    private void PlayOneShot(AudioClip clip, float volume)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip, volume);
        startTime = Time.time;
    }

    private bool IsPlaying()
    {
        if ((Time.time - startTime) >= boot.length - 0.75f)
        {
            return false;
        }
        return true;
    }
}
