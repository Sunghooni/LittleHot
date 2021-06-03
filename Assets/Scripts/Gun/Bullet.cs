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
        StartCoroutine(OncollisionCheck());
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

    private void BulletHitManage(GameObject collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyLife _EnemyLife = collision.GetComponent<EnemyLife>();
            _EnemyLife.isDead = true;
            _EnemyLife.deadByGun = true;
        }
        else if (collision.CompareTag("Player") && !timeScaleSO.isReplaying)
        {
            PlayerLife _PlayerLife = collision.GetComponent<PlayerLife>();
            _PlayerLife.DeadMotion();
        }
        Destroy(gameObject);
    }

    IEnumerator OncollisionCheck()
    {
        Vector3 prePosition = Vector3.zero;
        Vector3 curPosition = Vector3.zero;

        while (true)
        {
            if (prePosition == curPosition)
            {
                curPosition = gameObject.transform.position;
            }
            else
            {
                prePosition = curPosition;
                curPosition = gameObject.transform.position;

                Vector3 dir = (curPosition - prePosition).normalized;
                float dist = Vector3.Distance(prePosition, curPosition);
                
                if (Physics.Raycast(prePosition, dir, out RaycastHit hit, dist))
                {
                    BulletHitManage(hit.transform.gameObject);
                    break;
                }
                Debug.DrawRay(prePosition, dir * dist, Color.red, 10f);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
