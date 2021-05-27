using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PlayData
{
    public int vertInput;
    public int horzInput;
    public float mouseX;
    public float mouseY;
    public bool leftMouseBtn;
    public bool rightMouseBtn;
};

public class RecordPlayData : MonoBehaviour
{
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playDataSO.isReplayMode)
        {
            playDataSO.isReplayMode = true;
            SceneManager.LoadScene("MainScene");
        }
    }

    private void FixedUpdate()
    {
        if (!isReplaying)
        {
            RecordData();
        }
        else
        {
            Debug.Log(Time.timeScale);
            SetTransform();
        }
    }

    private void RecordData()
    {
        PlayData data = new PlayData
        {
            vertInput = int.Parse(Input.GetAxisRaw("Vertical").ToString()),
            horzInput = int.Parse(Input.GetAxisRaw("Horizontal").ToString()),
            mouseX = Input.GetAxis("Mouse X"),
            mouseY = Input.GetAxis("Mouse Y"),
            leftMouseBtn = Input.GetMouseButtonDown(0),
            rightMouseBtn = Input.GetMouseButtonDown(1)
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
            _PlayerRotate.mouseX = data.mouseX;
            _PlayerRotate.mouseY = data.mouseY;

            nowIndex++;
        }
    }
}
