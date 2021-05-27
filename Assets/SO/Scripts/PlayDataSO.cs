using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayDataSO : ScriptableObject
{
    public bool isReplayMode = false;
    public List<PlayData> records = new List<PlayData>();
}
