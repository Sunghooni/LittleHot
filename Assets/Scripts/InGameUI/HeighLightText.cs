using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HeighLightText : MonoBehaviour
{
    public GameObject defaultPostProcessing;
    public GameObject defaultCanvas;
    public GameObject heighlightCanvas;
    public TextMeshProUGUI heighlightText;
    public Volume UIvolume;

    private bool stopTime = true;
    private LensDistortion lens;

    public readonly string killThemAll = "KILL\nTHEM\nALL";
    public readonly string focusOnIdiot = "FOCUS\nON,\nIDIOT";
    public readonly string super = "SUPER";
    public readonly string hot = "HOT";
    public readonly string shoot = "SHOOT";
    public readonly string faster = "FASTER";

    private void Awake()
    {
        UIvolume.profile.TryGet(out lens);
    }

    public void StartShowText(params string[] texts)
    {
        heighlightText.text = "";
        defaultPostProcessing.SetActive(false);
        heighlightCanvas.SetActive(true);
        defaultCanvas.SetActive(false);

        StartCoroutine(SetText(texts));
    }

    private void PlayInGameSFX(string text)
    {
        if (text.Equals(super))
        {
            AudioManager.instance.PlaySound("SUPER", PlayerState.GetInstance().Player);
        }
        if (text.Equals(hot))
        {
            AudioManager.instance.PlaySound("HOT", PlayerState.GetInstance().Player);
        }
        else
        {
            AudioManager.instance.PlaySound("Chapter", gameObject);
        }
    }

    IEnumerator SetText(params string[] texts)
    {
        float preTimeScale = Time.timeScale;
        float delay = 2f / texts.Length;

        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(SetGameStop());

        for (int i = 0; i < texts.Length; i++)
        {
            heighlightText.text = texts[i];
            PlayInGameSFX(texts[i]);
            StartCoroutine(ShowTextEffects());
            
            yield return new WaitForSecondsRealtime(delay);
        }

        defaultPostProcessing.SetActive(true);
        defaultCanvas.SetActive(true);
        heighlightCanvas.SetActive(false);

        stopTime = false;
        Time.timeScale = preTimeScale;
        Time.fixedDeltaTime = preTimeScale * 0.02f;
    }

    IEnumerator ShowTextEffects()
    {
        float timer = 0;
        float startIntens = 0.6f;
        float toIntens = 0.25f;
        float lerpSpeed = 0.5f;
        float lerpTime = 0.3f;

        lens.intensity.value = startIntens;

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;
            lens.intensity.value = Mathf.Lerp(lens.intensity.value, toIntens, lerpSpeed);
            yield return null;
        }
    }

    IEnumerator SetGameStop()
    {
        stopTime = true;

        while (stopTime)
        {
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
            }
            yield return null;
        }
    }
}
