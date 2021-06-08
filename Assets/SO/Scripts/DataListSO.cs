using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataListSO : ScriptableObject
{
    public List<PlayerDataStruct> playerRecord = new List<PlayerDataStruct>();

    public List<CameraDataStruct> cameraRecord = new List<CameraDataStruct>();

    public List<EnemyDataStruct[]> enemyRecord = new List<EnemyDataStruct[]>();

    public List<GunDataStruct[]> gunRecord = new List<GunDataStruct[]>();

    public List<BulletDataStruct[]> bulletRecord = new List<BulletDataStruct[]>();
}

public struct PlayerDataStruct
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isPunching1;
    public bool isPunching2;
};

public struct CameraDataStruct
{
    public Vector3 position;
    public Quaternion rotation;
};

public struct EnemyDataStruct
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isDead;
    public bool isAttacking;
};

public struct GunDataStruct
{
    public Vector3 position;
    public Quaternion rotation;
};

public struct BulletDataStruct
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
};