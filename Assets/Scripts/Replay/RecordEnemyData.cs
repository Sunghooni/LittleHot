using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordEnemyData : MonoBehaviour
{
    public EnemyDataSO enemyDataSO;
    private int nowIndex = 0;
    private bool isReplaying = false;

    private void Awake()
    {
        if (!enemyDataSO.isReplayMode)
        {
            GetEnemys();
            enemyDataSO.records.Clear();
        }
        else
        {
            GetEnemys();
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            isReplaying = true;
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
            enemyDataSO.isReplayMode = !enemyDataSO.isReplayMode;
        }
    }

    private void GetEnemys()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDataSO.enemyList = objs;

        if (isReplaying)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                enemyDataSO.enemyList[i].GetComponent<EnemyMove>().isReplay = true;
            }
        }
    }

    IEnumerator Replay()
    {
        while (true)
        {
            if (!isReplaying)
            {
                RecordData();
            }
            else
            {
                SetTransform();
            }
            yield return new WaitForSeconds(0.0001f / Time.timeScale);
        }
    }

    private void RecordData()
    {
        EnemyData[] datas = new EnemyData[enemyDataSO.enemyList.Length];

        for (int i = 0; i < enemyDataSO.enemyList.Length; i++)
        {
            EnemyData data = new EnemyData
            {
                enemyPos = enemyDataSO.enemyList[i].transform.position,
                enemyRot = enemyDataSO.enemyList[i].transform.rotation
            };
            datas[i] = data;
        }

        enemyDataSO.records.Add(datas);
    }

    private void SetTransform()
    {
        if (nowIndex < enemyDataSO.records.Count)
        {
            EnemyData[] datas = enemyDataSO.records[nowIndex];

            for (int i = 0; i < enemyDataSO.enemyList.Length; i++)
            {
                enemyDataSO.enemyList[i].transform.position = datas[i].enemyPos;
                enemyDataSO.enemyList[i].transform.rotation = datas[i].enemyRot;
            }

            nowIndex++;
        }
    }
}
