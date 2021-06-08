using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPuller : MonoBehaviour
{
    public static BulletPuller puller;
    public GameObject bullets;
    public int idx = 0;

    private void Awake()
    {
        puller = gameObject.GetComponent<BulletPuller>();
    }

    public GameObject PullBullet()
    {
        if (idx >= bullets.transform.childCount)
        {
            idx = 0;
        }

        return bullets.transform.GetChild(idx++).gameObject;
    }
}
