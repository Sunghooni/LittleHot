using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordGunData : MonoBehaviour
{
    public GunDataSO gunDataSO;
    public GameObject[] guns;
    private int nowIndex = 0;

    private void Awake()
    {
        GetGuns();
    }

    private void GetGuns()
    {
        gunDataSO.gunList = guns;
    }

    public void RecordData()
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

    public void SetTransform()
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
