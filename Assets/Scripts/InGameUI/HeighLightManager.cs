using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeighLightManager : MonoBehaviour
{
    public PlayModeSO playModeSO;
    public HeighLightText heighlightText;
    public GameObject[] enemys;

    [HideInInspector]
    public float deadCount = 0;

    private bool playFirstText = false;
    private List<EnemyLife> enemyLifes = new List<EnemyLife>();
    private List<int> heighlights = new List<int>();

    private void Awake()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyLifes.Add(enemys[i].GetComponent<EnemyLife>());
        }
        if (playModeSO.isReplayMode)
        {
            SetHeighlightParts();
        }
    }

    private void Update()
    {
        CheckEnemyDead();

        if (playModeSO.isReplayMode)
        {
            PlayHeighlights();
        }
        else
        {
            PlayStartText();
        }
    }

    private void CheckEnemyDead()
    {
        for (int i = 0; i < enemyLifes.Count; i++)
        {
            if (enemyLifes[i].isDead)
            {
                deadCount++;
                enemyLifes.RemoveAt(i);
                break;
            }
        }
    }

    private void SetHeighlightParts()
    {
        for(int i = 1; i <= enemys.Length; i += 3)
        {
            heighlights.Add(i);
        }
    }

    private void PlayStartText()
    {
        if (deadCount == 1 && !playFirstText)
        {
            heighlightText.StartShowText(heighlightText.killThemAll);
            playFirstText = true;
        }
        else if (deadCount == 4 && playFirstText)
        {
            heighlightText.StartShowText(heighlightText.shoot, heighlightText.faster);
            playFirstText = false;
        }
    }

    private void PlayHeighlights()
    {
        if (heighlights.Contains((int)deadCount))
        {
            heighlightText.StartShowText(heighlightText.super, heighlightText.hot);
            heighlights.Remove((int)deadCount);
        }
    }
}
