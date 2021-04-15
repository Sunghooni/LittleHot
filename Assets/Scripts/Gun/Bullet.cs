using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float flySpeed = 10f;
    private readonly float lifeTime = 5f;

    private void Start()
    {
        StartCoroutine(nameof(BulletLifeTimer));
    }

    private void FixedUpdate()
    {
        gameObject.transform.Translate(new Vector3(0, 0, 1) * flySpeed * Time.deltaTime);
    }

    IEnumerator BulletLifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
