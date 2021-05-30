using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordGunData : MonoBehaviour
{
    public GunDataSO gunDataSO;
    private int nowIndex = 0;
    private bool isReplaying = false;

    private void Awake()
    {
        if (!gunDataSO.isReplayMode)
        {
            GetGuns();
            gunDataSO.records.Clear();
        }
        else
        {
            GetGuns();
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
            gunDataSO.isReplayMode = !gunDataSO.isReplayMode;
        }
    }

    private void GetGuns()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Gun");
        gunDataSO.gunList = objs;
        print(".");
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
        GunData[] datas = new GunData[gunDataSO.gunList.Length];

        for (int i = 0; i < gunDataSO.gunList.Length; i++)
        {
            GunData data = new GunData
            {
                gunPos = gunDataSO.gunList[i].transform.position,
                gunRot = gunDataSO.gunList[i].transform.rotation
            };
            datas[i] = data;
        }

        gunDataSO.records.Add(datas);
    }

    private void SetTransform()
    {
        if (nowIndex < gunDataSO.records.Count)
        {
            GunData[] datas = gunDataSO.records[nowIndex];

            for (int i = 0; i < gunDataSO.gunList.Length; i++)
            {
                gunDataSO.gunList[i].transform.position = datas[i].gunPos;
                gunDataSO.gunList[i].transform.rotation = datas[i].gunRot;
            }

            nowIndex++;
        }
    }
}
