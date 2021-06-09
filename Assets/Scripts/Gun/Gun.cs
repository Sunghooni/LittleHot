using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("State")]
    public bool isCatching = false;
    public bool isHolded = false;
    public bool isThrowing = false;

    private Camera mainCamera;
    private GameObject player;
    private GameObject playerHand;
    private GameObject handIK;
    private Transform tr;
    private Rigidbody rb;
    private Transform playerTr;

    private readonly float shotRayLength = 100f;
    private const float throwPower = 10;
    private const float fixedRotY = 90;
    private PlayerState playerState;

    private void Awake()
    {
        tr = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();

        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        playerState = PlayerState.GetInstance();
        player = playerState.Player;
        playerTr = player.GetComponent<Transform>();
        playerHand = playerState.gunAimPos;
        handIK = playerState.gunThrowPos;
    }

    private void Update()
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

        isThrowing = false;
        isCatching = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        playerState.isHolding = true;
        gameObject.GetComponent<MeshCollider>().isTrigger = true;

        AudioManager.instance.PlaySound("GrabGunSFX", gameObject);

        while (timer <= 1f)
        {
            timer += Time.deltaTime * 2;

            Vector3 angle = playerHand.transform.eulerAngles;
            Vector3 toPos = playerHand.transform.position;
            Vector3 toRot = new Vector3(0, angle.y, angle.z);

            tr.position = Vector3.Lerp(originPos, toPos, timer);
            tr.eulerAngles = Vector3.Lerp(originRot, toRot, timer);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            yield return new WaitForFixedUpdate();
        }

        isHolded = true;
        isCatching = false;
        playerState.isActing = false;
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
        GameObject bullet = BulletPuller.puller.PullBullet();

        bullet.transform.position = startPos;
        bullet.transform.LookAt(GetTarget());
        bullet.SetActive(true);

        AudioManager.instance.PlaySound("PlayerGunFireSFX", bullet);
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
            if(timer < 0)
            {
                deltaTime = (timer - deltaTime) * -1;
                motionPlay = false;
            }

            playerHand.transform.position += transform.up * deltaTime * motionSpeed;
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
        float handFollowSpeed = 3f;
        isHolded = false;
        isThrowing = true;

        while (timer < delay)
        {
            Vector3 gunPos = gameObject.transform.position;
            Vector3 handPos = handIK.transform.position;

            timer += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gunPos, handPos, timer * handFollowSpeed);
            yield return new WaitForFixedUpdate();
        }

        playerState.isHolding = false;
        playerState.holdingObj = null;
        gameObject.transform.LookAt(GetTarget());
        gameObject.transform.Rotate(Vector3.up * -fixedRotY);

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(transform.right * throwPower, ForceMode.Impulse);
        gameObject.GetComponent<MeshCollider>().isTrigger = false;
    }

    private Vector3 GetTarget()
    {
        Vector3 target;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, shotRayLength))
        {
            target = hit.point;
        }
        else
        {
            target = mainCamera.transform.position + mainCamera.transform.forward * shotRayLength;
        }

        return target;
    }

    public void ThrowGunByAttack()
    {
        float gunThrowPower = 5f;
        Rigidbody holdingGunRigid = gameObject.transform.GetComponent<Rigidbody>();

        holdingGunRigid.useGravity = true;
        holdingGunRigid.isKinematic = false;
        holdingGunRigid.AddForce(transform.up * gunThrowPower, ForceMode.Impulse);

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
