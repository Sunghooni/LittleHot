using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyData
{
    public Vector3 enemyPos;
    public Quaternion enemyRot;
    public bool isShooting;
};

[CreateAssetMenu]
public class EnemyDataSO : ScriptableObject
{
    public bool isReplayMode = false;
    public GameObject[] enemyList;
    public List<EnemyData[]> records = new List<EnemyData[]>();
}
