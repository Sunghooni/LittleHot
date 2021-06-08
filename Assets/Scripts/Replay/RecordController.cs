using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordController : MonoBehaviour
{
    public PlayDataSO playDataSO;
    public RecordData recordData;
    public ReplayData replayData;

    private void Awake()
    {
        if (!playDataSO.isReplayMode)
        {
            recordData.BasicSetting();
        }
        if (playDataSO.isReplayMode)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            replayData.BasicSetting();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playDataSO.isReplayMode = !playDataSO.isReplayMode;
            SceneManager.LoadScene("PlayScene");
        }
    }
}
