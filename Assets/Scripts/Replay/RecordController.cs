using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordController : MonoBehaviour
{
    public PlayModeSO playModeSO;
    public RecordData recordData;
    public ReplayData replayData;

    private void Awake()
    {
        if (!playModeSO.isReplayMode)
        {
            recordData.BasicSetting();
        }
        if (playModeSO.isReplayMode)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            replayData.BasicSetting();
        }
    }
}
