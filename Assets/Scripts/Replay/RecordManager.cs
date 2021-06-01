using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordManager : MonoBehaviour
{
    public RecordPlayData recordPlayData;
    public RecordEnemyData recordEnemyData;
    public RecordGunData recordGunData;

    private void Awake()
    {
        if (!recordEnemyData.enemyDataSO.isReplayMode)
        {
            recordPlayData.playDataSO.records.Clear();
            recordGunData.gunDataSO.records.Clear();
            recordEnemyData.enemyDataSO.records.Clear();
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    private void Start()
    {
        StartCoroutine(nameof(Replay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            recordPlayData.playDataSO.isReplayMode = !recordPlayData.playDataSO.isReplayMode;
            recordEnemyData.enemyDataSO.isReplayMode = !recordEnemyData.enemyDataSO.isReplayMode;
            recordGunData.gunDataSO.isReplayMode = !recordGunData.gunDataSO.isReplayMode;

            SceneManager.LoadScene("MainScene");
        }
    }

    IEnumerator Replay()
    {
        while (true)
        {
            if (!recordEnemyData.enemyDataSO.isReplayMode)
            {
                recordPlayData.RecordData();
                recordEnemyData.RecordData();
                recordGunData.RecordData();
                yield return new WaitForSeconds(0.0001f / Time.timeScale);
            }
            else
            {
                recordPlayData.SetTransform();
                recordEnemyData.SetTransform();
                recordGunData.SetTransform();
                yield return new WaitForSeconds(0.00005f / Time.timeScale);
            }
        }
    }
}
