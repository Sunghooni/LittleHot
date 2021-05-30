using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GunData
{
    public Vector3 gunPos;
    public Quaternion gunRot;
};

[CreateAssetMenu]
public class GunDataSO : ScriptableObject
{
    public bool isReplayMode = false;
    public GameObject[] gunList;
    public List<GunData[]> records = new List<GunData[]>();
}
