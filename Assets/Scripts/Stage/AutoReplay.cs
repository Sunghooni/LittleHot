using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoReplay : MonoBehaviour
{
    public HeighLightManager heighlightManager;
    public PlayModeSO playModeSO;
    public GameObject pressSpace;

    private float timer = 0;

    private void Update()
    {
        CheckIsGameCleared();
    }

    private void CheckIsGameCleared()
    {
        float enemyLeft = heighlightManager.enemys.Length - heighlightManager.deadCount;
        timer += Time.deltaTime;

        if (!playModeSO.isReplayMode && enemyLeft == 0)
        {
            if (timer > 20)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ReplayScene();
                }
                else
                {
                    pressSpace.SetActive(true);
                }
            }
        }
        else if (playModeSO.isReplayMode && enemyLeft == 0)
        {
            Invoke(nameof(ExitPlayScene), 2f);
        }
    }

    private void ReplayScene()
    {
        playModeSO.isReplayMode = true;
        SceneManager.LoadScene("PlayScene");
    }

    private void ExitPlayScene()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("MainScene");
    }
}
