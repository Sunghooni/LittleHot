using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TimeScaleDataSO timeScaleSO;
    private float speed = 10f;
    private int idx = 0;
    private readonly float flySpeed = 10f;
    private readonly float lifeTime = 5f;

    private void Start()
    {
        if (timeScaleSO.isReplaying)
        {
            //StartCoroutine(SetBulletFlySpeed());
        }
        StartCoroutine(nameof(BulletLifeTimer));
    }

    private void FixedUpdate()
    {
        gameObject.transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
    }

    IEnumerator BulletLifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    IEnumerator SetBulletFlySpeed()
    {
        while (idx < timeScaleSO.timeScaleList.Count)
        {
            //speed = Mathf.Lerp(speed, flySpeed * timeScaleSO.timeScaleList[idx], 0.5f);
            speed = flySpeed * timeScaleSO.timeScaleList[idx];
            idx++;
            yield return new WaitForSeconds(0.00005f / Time.timeScale);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyLife _EnemyLife = collision.gameObject.GetComponent<EnemyLife>();
            _EnemyLife.isDead = true;
            _EnemyLife.deadByGun = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLife _PlayerLife = collision.gameObject.GetComponent<PlayerLife>();
            _PlayerLife.DeadMotion();
        }
        Destroy(gameObject);
    }
}
