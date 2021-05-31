using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordPlayData : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public PlayDataSO playDataSO;
    public PlayerMove _PlayerMove;
    public PlayerRotate _PlayerRotate;
    public PlayerAct _PlayerAct;
    public TimeManager _TimeManager;

    private int nowIndex = 0;
    private bool isReplaying = false;

    private void Awake()
    {
        if (!playDataSO.isReplayMode)
        {
            playDataSO.records.Clear();
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            isReplaying = true;
            _PlayerMove.isReplaying = true;
            _PlayerAct.isReplaying = true;
            _PlayerRotate.isReplaying = true;
            _TimeManager.isReplaying = true;
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
            playDataSO.isReplayMode = !playDataSO.isReplayMode;
            SceneManager.LoadScene("MainScene");
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
        PlayData data = new PlayData
        {
            vertInput = int.Parse(Input.GetAxisRaw("Vertical").ToString()),
            horzInput = int.Parse(Input.GetAxisRaw("Horizontal").ToString()),
            playerPos = player.transform.position,
            playerRot = player.transform.rotation,
            cameraRot = camera.transform.rotation,
            leftMouseBtn = Input.GetMouseButton(0),
            rightMouseBtn = Input.GetMouseButton(1)
        };

        playDataSO.records.Add(data);
    }

    private void SetTransform()
    {
        if (nowIndex < playDataSO.records.Count)
        {
            PlayData data = playDataSO.records[nowIndex];

            _PlayerMove.vert = data.vertInput;
            _PlayerMove.horz = data.horzInput;
            _PlayerAct.isLeftButtonClicked = data.leftMouseBtn;
            _PlayerAct.isRightButtonClicked = data.rightMouseBtn;

            _PlayerMove.SetPosition(data.playerPos);
            _PlayerRotate.SetRotation(data.playerRot, data.cameraRot);

            nowIndex++;
        }
    }
}
