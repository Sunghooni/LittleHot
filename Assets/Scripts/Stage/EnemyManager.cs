﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemyIdx = 0;

    private void Awake()
    {
        StartCoroutine(EnemyWaveInstant());
    }

    private void SetEnemyAcive()
    {
        if (gameObject.transform.GetChild(enemyIdx).childCount == 0) return;

        Transform enemy = gameObject.transform.GetChild(enemyIdx).GetChild(0);
        enemy.gameObject.SetActive(true);
        enemy.SetParent(null);

        enemyIdx++;
        enemyIdx %= gameObject.transform.childCount;
    }

    IEnumerator EnemyWaveInstant()
    {
        float waveNum = 3f;
        float waveDelay = 5f;

        for (int i = 0; i < waveNum; i++)
        {
            SetEnemyAcive();
            SetEnemyAcive();
            SetEnemyAcive();

            yield return new WaitForSeconds(waveDelay);
        }
    }
}
