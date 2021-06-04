using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordManager : MonoBehaviour
{
    public TimeScaleDataSO timeScaleData;
    public RecordPlayData recordPlayData;
    public RecordEnemyData recordEnemyData;
    public RecordGunData recordGunData;

    private void Awake()
    {
        if (!recordEnemyData.enemyDataSO.isReplayMode)
        {
            timeScaleData.timeScaleList.Clear();
            recordPlayData.playDataSO.records.Clear();
            recordGunData.gunDataSO.records.Clear();
            recordEnemyData.enemyDataSO.records.Clear(); 
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
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
            timeScaleData.isReplaying = !timeScaleData.isReplaying;
            recordPlayData.playDataSO.isReplayMode = !recordPlayData.playDataSO.isReplayMode;
            recordEnemyData.enemyDataSO.isReplayMode = !recordEnemyData.enemyDataSO.isReplayMode;
            recordGunData.gunDataSO.isReplayMode = !recordGunData.gunDataSO.isReplayMode;

            SceneManager.LoadScene("PlayScene");
        }
    }

    IEnumerator Replay()
    {
        while (true)
        {
            if (!recordEnemyData.enemyDataSO.isReplayMode)
            {
                timeScaleData.timeScaleList.Add(Time.timeScale);
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
                yield return new WaitForSeconds(0.0001f / Time.timeScale);
            }
        }
    }
}

/*
PlayerPos
PlayerRot
CameraPos
CameraRot
EnemyPos
EnemyRot
EnemyState
GunPos
GunRot
BulletPos
BulletRot
*/