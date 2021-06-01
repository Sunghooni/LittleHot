using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if (playDataSO.isReplayMode)
        {
            _PlayerMove.isReplaying = true;
            _PlayerAct.isReplaying = true;
            _PlayerRotate.isReplaying = true;
            _TimeManager.isReplaying = true;
        }
    }

    public void RecordData()
    {
        if (player == null) return;

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

    public void SetTransform()
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
