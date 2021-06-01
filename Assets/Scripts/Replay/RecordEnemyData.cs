using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordEnemyData : MonoBehaviour
{
    public EnemyDataSO enemyDataSO;
    public GameObject[] enemys;
    private int nowIndex = 0;

    private void Awake()
    {
        GetEnemys();
    }

    private void GetEnemys()
    {
        enemyDataSO.enemyList = enemys;

        if (enemyDataSO.isReplayMode)
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                enemyDataSO.enemyList[i].GetComponent<EnemyMove>().isReplay = true;
            }
        }
    }

    public void RecordData()
    {
        EnemyData[] datas = new EnemyData[enemyDataSO.enemyList.Length];

        for (int i = 0; i < enemyDataSO.enemyList.Length; i++)
        {
            if (enemyDataSO.enemyList[i] == null) continue;

            EnemyData data = new EnemyData
            {
                enemyPos = enemyDataSO.enemyList[i].transform.position,
                enemyRot = enemyDataSO.enemyList[i].transform.rotation,
                isShooting = enemyDataSO.enemyList[i].GetComponent<EnemyMove>().isShooting
            };
            datas[i] = data;
        }

        enemyDataSO.records.Add(datas);
    }

    public void SetTransform()
    {
        if (nowIndex < enemyDataSO.records.Count)
        {
            EnemyData[] datas = enemyDataSO.records[nowIndex];

            for (int i = 0; i < enemyDataSO.enemyList.Length; i++)
            {
                if (enemyDataSO.enemyList[i] == null) continue;

                enemyDataSO.enemyList[i].transform.position = datas[i].enemyPos;
                enemyDataSO.enemyList[i].transform.rotation = datas[i].enemyRot;
                enemyDataSO.enemyList[i].GetComponent<EnemyMove>().isShooting = datas[i].isShooting;
            }

            nowIndex++;
        }
    }
}
