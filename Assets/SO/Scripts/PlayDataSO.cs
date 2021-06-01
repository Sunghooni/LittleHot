using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayData
{
    public int vertInput;
    public int horzInput;
    public Vector3 playerPos;
    public Quaternion playerRot;
    public Quaternion cameraRot;
    public bool leftMouseBtn;
    public bool rightMouseBtn;
};

[CreateAssetMenu]
public class PlayDataSO : ScriptableObject
{
    public bool isReplayMode = false;
    public List<PlayData> records = new List<PlayData>();
}
