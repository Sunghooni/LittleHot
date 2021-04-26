using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private PlayerState playerState;
    private float timeScale;

    private const float normalTimeScale = 1f;
    private const float slowerTimeScale = 0.05f;
    private const float slowestTimeScale = 0.02f;

    private void Awake()
    {
        playerState = PlayerState.GetInstance();
    }

    private void Update()
    {
        CheckMotionPlaying();
        ChangeTimeScale();
    }

    private void CheckMotionPlaying()
    {
        if (playerState.isActing)
        {
            timeScale = normalTimeScale;
        }
        else if (playerState.isMotion)
        {
            timeScale = normalTimeScale;
        }
        else if (playerState.isRotate)
        {
            timeScale = slowerTimeScale;
        }
        else
        {
            timeScale = slowestTimeScale;
        }
    }

    private void ChangeTimeScale()
    {
        float lerpSpeed = timeScale != 1 ? 0.5f : 1f;

        if (Time.timeScale != timeScale)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, lerpSpeed);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        Debug.Log(timeScale);
    }
}
