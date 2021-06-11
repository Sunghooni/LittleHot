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
    
    private LensDistortion lens;
    private const string super = "SUPER";
    private const string hot = "HOT";
    private const string killThemAll = "KILL\nTHEM\nALL";

    private void Awake()
    {
        UIvolume.profile.TryGet(out lens);
    }

    private void Update()
    {
        StartShowText();
    }

    private void StartShowText()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            defaultPostProcessing.SetActive(false);
            heighlightCanvas.SetActive(true);
            defaultCanvas.SetActive(false);

            StartCoroutine(SetText(super, hot));
            //StartCoroutine(SetText(killThemAll));
        }
    }

    IEnumerator SetText(params string[] texts)
    {
        float preTimeScale = Time.timeScale;
        float delay = 1f;
        
        Time.timeScale = 0;

        for (int i = 0; i < texts.Length; i++)
        {
            heighlightText.text = texts[i];
            StartCoroutine(ShowTextEffects());
            
            yield return new WaitForSecondsRealtime(delay);
        }

        defaultPostProcessing.SetActive(true);
        defaultCanvas.SetActive(true);
        heighlightCanvas.SetActive(false);

        Time.timeScale = preTimeScale;
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
}
