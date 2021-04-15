using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;

    [Header("State")]
    public bool isCatching = false;
    public bool isHolded = false;
    public bool isThrowing = false;

    private GameObject player;
    private GameObject playerHand;
    private GameObject handIK;
    private Transform tr;
    private Rigidbody rb;
    private Transform playerTr;

    private const float throwPower = 10;
    private const float fixedRotY = 90;
    private PlayerState playerState;

    private void Awake()
    {
        tr = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();

        playerState = PlayerState.GetInstance();
        player = playerState.Player;
        playerTr = player.GetComponent<Transform>();
        playerHand = playerState.gunAimPos;
        handIK = playerState.gunThrowPos;
    }

    private void FixedUpdate()
    {
        if (isHolded)
        {
            HoldingToHand();
        }
    }

    public void HoldedToHand()
    {
        StartCoroutine(MoveToHand());
    }

    IEnumerator MoveToHand()
    {
        float timer = 0;
        Vector3 originPos = tr.position;
        Vector3 originRot = tr.eulerAngles;

        isCatching = true;
        rb.useGravity = false;
        playerState.isHolding = true;
        gameObject.GetComponent<MeshCollider>().isTrigger = true;

        while (timer <= 1f)
        {
            timer += Time.deltaTime * 2;

            Vector3 angle = playerHand.transform.eulerAngles;
            Vector3 toPos = playerHand.transform.position;
            Vector3 toRot = new Vector3(0, angle.y, angle.z);

            tr.position = Vector3.Lerp(originPos, toPos, timer);
            tr.eulerAngles = Vector3.Lerp(originRot, toRot, timer);

            yield return new WaitForFixedUpdate();
        }

        isHolded = true;
        isCatching = false;
    }

    private void HoldingToHand()
    {
        Vector3 angle = playerHand.transform.eulerAngles;

        tr.position = playerHand.transform.position;
        tr.eulerAngles = new Vector3(0, angle.y, angle.z);
    }

    public void ShotGun()
    {
        FlyBullet();
        StartCoroutine(ShotMotion());
    }

    private void FlyBullet()
    {
        Vector3 startPos = gameObject.transform.GetChild(0).position;
        Vector3 startRot = gameObject.transform.eulerAngles + Vector3.up * fixedRotY;

        startRot += Vector3.left * gameObject.transform.eulerAngles.z;
        Instantiate(Bullet, startPos, Quaternion.Euler(startRot));
    }

    IEnumerator ShotMotion()
    {
        float motionTime = 0.6f;
        float motionSpeed = 0.05f;
        float timer = 0;
        bool isUp = true;

        while (timer >= 0)
        {
            if (timer > motionTime * 0.5f)
            {
                isUp = false;
            }

            var deltaTime = isUp ? Time.deltaTime : -Time.deltaTime;
            timer += deltaTime;

            playerHand.transform.position += Vector3.up * deltaTime * motionSpeed;
            yield return new WaitForFixedUpdate();
        }

        playerState.animator.SetBool("Shot", false);
    }

    public void Throwing()
    {
        StartCoroutine(nameof(ThrowFix));
    }

    IEnumerator ThrowFix()
    {
        float timer = 0;
        float delay = 0.5f;
        isHolded = false;
        isThrowing = true;

        while (timer < delay)
        {
            Vector3 gunPos = gameObject.transform.position;
            Vector3 handPos = handIK.transform.position;

            timer += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gunPos, handPos, timer * 3);
            yield return new WaitForFixedUpdate();
        }

        playerState.isHolding = false;
        playerState.holdingObj = null;

        rb.useGravity = true;
        rb.AddForce(transform.right * throwPower, ForceMode.Impulse);
        gameObject.GetComponent<MeshCollider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowing)
        {
            isThrowing = false;
        }
    }
}
