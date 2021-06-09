using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunForEnemy : MonoBehaviour
{
    public GameObject Player;
    public GameObject Bullet;

    public void ShotGun()
    {
        FlyBullet();
        StartCoroutine(ShotMotion());
    }

    private void FlyBullet()
    {
        Vector3 startPos = gameObject.transform.GetChild(0).position;
        GameObject bullet = BulletPuller.puller.PullBullet();

        bullet.transform.position = startPos;
        bullet.transform.LookAt(GetTarget());
        bullet.SetActive(true);

        AudioManager.instance.PlaySound("EnemyGunFireSFX", gameObject);
    }

    IEnumerator ShotMotion()
    {
        float motionTime = 0.15f;
        float motionSpeed = 0.1f;
        float timer = 0;

        bool motionPlay = true;
        bool isUp = true;

        while (motionPlay)
        {
            var deltaTime = isUp ? Time.deltaTime : -Time.deltaTime;
            timer += deltaTime;

            if (timer > motionTime)
            {
                deltaTime = motionTime - (timer - deltaTime);
                timer = motionTime;
                isUp = false;
            }
            if (timer < 0)
            {
                deltaTime = (timer - deltaTime) * -1;
                motionPlay = false;
            }

            gameObject.transform.position += transform.up * deltaTime * motionSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 GetTarget()
    {
        return Player.transform.position + Vector3.up;
    }
}
