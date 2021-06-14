using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayData : MonoBehaviour
{
    public DataListSO dataListSO;
    public InGameObjects inGameObjects;
    public TimeManager timeManager;
    public PlayerAct playerAct;

    private GameObject player;
    private Animator playerAnimator;

    private GameObject camera;
    private GameObject[] enemyList;
    private GameObject[] gunList;
    private GameObject[] bulletList;

    private int idx = 0;

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
        //set Player cannot move by input
        player.GetComponent<PlayerMove>().isReplaying = true;
        player.GetComponent<PlayerRotate>().isReplaying = true;
        playerAct.isReplaying = true;

        //fix timescale to 1
        timeManager.isReplaying = true;

        //set enemy not to move itself
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].GetComponent<EnemyMove>().isReplay = true;
        }

        StartCoroutine(Replay());
    }

    IEnumerator Replay()
    {
        Debug.Log("Replaying");

        while (true)
        {
            if (idx >= dataListSO.playerRecord.Count) break;

            SetPlayerData();
            SetCameraData();
            SetEnemyData();
            SetGunData();
            SetBulletData();
            SetAudioData();

            idx++;
            
            while (Time.timeScale == 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.0005f / Time.timeScale);
        }
    }

    private void SetPlayerData()
    {
        player.transform.position = dataListSO.playerRecord[idx].position;
        player.transform.rotation = dataListSO.playerRecord[idx].rotation;

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") 
            && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
        {
            if (dataListSO.playerRecord[idx].isPunching1)
            {
                playerAnimator.Play("Hit1");
            }
            if (dataListSO.playerRecord[idx].isPunching2)
            {
                playerAnimator.Play("Hit2");
            }
        }
    }

    private void SetCameraData()
    {
        camera.transform.position = dataListSO.cameraRecord[idx].position;
        camera.transform.rotation = dataListSO.cameraRecord[idx].rotation;
    }

    private void SetEnemyData()
    {
        EnemyDataStruct[] data = dataListSO.enemyRecord[idx];

        for (int i = 0; i < data.Length; i++)
        {
            if (enemyList[i] == null) continue;

            enemyList[i].transform.position = data[i].position;
            enemyList[i].transform.rotation = data[i].rotation;
            enemyList[i].GetComponent<EnemyLife>().isDead = data[i].isDead;
            enemyList[i].GetComponent<EnemyMove>().isShooting = data[i].isAttacking;
        }
    }

    private void SetGunData()
    {
        GunDataStruct[] data = dataListSO.gunRecord[idx];

        for (int i = 0; i < data.Length; i++)
        {
            gunList[i].transform.position = data[i].position;
            gunList[i].transform.rotation = data[i].rotation;
        }
    }

    private void SetBulletData()
    {
        BulletDataStruct[] data = dataListSO.bulletRecord[idx];

        for (int i = 0; i < data.Length; i++)
        {
            bulletList[i].SetActive(data[i].isActive);
            bulletList[i].transform.position = data[i].position;
            bulletList[i].transform.rotation = data[i].rotation;
        }
    }

    private void SetAudioData()
    {
        List<AudioDataStruct> list = dataListSO.audioRecord[idx];

        if (list.Count == 0) return;

        for (int i = 0; i < list.Count; i++)
        {
            GameObject source = GameObject.Find(list[i].source);

            AudioManager.instance.PlaySound(list[i].clipName, source);
        }
    }
}
