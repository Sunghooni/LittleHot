using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData : MonoBehaviour
{
    public DataListSO dataListSO;
    public InGameObjects inGameObjects;

    private GameObject player;
    private Animator playerAnimator;

    private GameObject camera;
    private GameObject[] enemyList;
    private GameObject[] gunList;
    private GameObject[] bulletList;

    private void Awake()
    {
        player = inGameObjects.player;
        playerAnimator = player.GetComponent<Animator>();
        camera = inGameObjects.camera;
        enemyList = inGameObjects.enemys;
        gunList = inGameObjects.guns;
        bulletList = inGameObjects.bullets;
    }

    public void BasicSetting()
    {
        dataListSO.playerRecord.Clear();
        dataListSO.cameraRecord.Clear();
        dataListSO.enemyRecord.Clear();
        dataListSO.gunRecord.Clear();
        dataListSO.bulletRecord.Clear();
        dataListSO.audioRecord.Clear();

        StartCoroutine(SaveData());
    }

    IEnumerator SaveData()
    {
        Debug.Log("Recording");

        while (true)
        {
            SavePlayerData();
            SaveCameraData();
            SaveEnemyData();
            SaveGunData();
            SaveBulletData();
            SaveAudioData();

            while (Time.timeScale == 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.0005f / Time.timeScale);
        }
    }

    private void SavePlayerData()
    {
        if (player == null) return;

        PlayerDataStruct playerInfo = new PlayerDataStruct
        {
            position = player.transform.position,
            rotation = player.transform.rotation,
            isPunching1 = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"),
            isPunching2 = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit2")
        };

        dataListSO.playerRecord.Add(playerInfo);
    }

    private void SaveCameraData()
    {
        if (camera == null) return;

        CameraDataStruct playerInfo = new CameraDataStruct
        {
            position = camera.transform.position,
            rotation = camera.transform.rotation
        };

        dataListSO.cameraRecord.Add(playerInfo);
    }

    private void SaveEnemyData()
    {
        EnemyDataStruct[] array = new EnemyDataStruct[inGameObjects.enemys.Length];

        if (inGameObjects.enemys == null || enemyList == null) return;

        for (int i = 0; i < inGameObjects.enemys.Length; i++)
        {
            if (enemyList[i] == null) continue;

            EnemyDataStruct enemyInfo = new EnemyDataStruct
            {
                position = enemyList[i].transform.position,
                rotation = enemyList[i].transform.rotation,
                isDead = enemyList[i].GetComponent<EnemyLife>().isDead,
                isAttacking = enemyList[i].GetComponent<EnemyMove>().isShooting
            };

            array[i] = enemyInfo;
        }

        dataListSO.enemyRecord.Add(array);
    }

    private void SaveGunData()
    {
        GunDataStruct[] array = new GunDataStruct[inGameObjects.guns.Length];

        if (inGameObjects.guns == null || gunList == null) return;

        for (int i = 0; i < inGameObjects.guns.Length; i++)
        {
            GunDataStruct gunInfo = new GunDataStruct
            {
                position = gunList[i].transform.position,
                rotation = gunList[i].transform.rotation
            };

            array[i] = gunInfo;
        }

        dataListSO.gunRecord.Add(array);
    }

    private void SaveBulletData()
    {
        BulletDataStruct[] array = new BulletDataStruct[inGameObjects.bullets.Length];

        if (inGameObjects.bullets == null || bulletList == null) return;

        for (int i = 0; i < inGameObjects.bullets.Length; i++)
        {
            if (bulletList[i] == null) continue;

            BulletDataStruct bulletInfo = new BulletDataStruct
            {
                position = bulletList[i].transform.position,
                rotation = bulletList[i].transform.rotation,
                isActive = bulletList[i].activeSelf
            };

            array[i] = bulletInfo;
        }

        dataListSO.bulletRecord.Add(array);
    }

    private void SaveAudioData()
    {
        dataListSO.audioRecord.Add(new List<AudioDataStruct>(AudioManager.instance.list));
        AudioManager.instance.list.Clear();
    }
}
