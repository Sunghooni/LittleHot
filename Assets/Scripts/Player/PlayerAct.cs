using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerState playerState;
    private Animator animator;
    private GameObject holdingObj;
    private bool isActing;
    private bool isHolding;
    private bool punched = false;

    private const int shotRayLength = 100;
    private const int nearAttackLength = 2;

    public bool isReplaying = false;
    public bool isLeftButtonClicked = false;
    public bool isRightButtonClicked = false;

    private void Awake()
    {
        playerState = PlayerState.GetInstance();
        animator = gameObject.GetComponent<Animator>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        holdingObj = playerState.holdingObj;
        isActing = playerState.isActing;
        isHolding = playerState.isHolding;

        if (!isReplaying)
        {
            GetMouseButtonClick();
        }

        CheckPunchType();
        ThrowMotion();
        LeftMouseBtnClick();
    }

    private void GetMouseButtonClick()
    {
        isLeftButtonClicked = Input.GetMouseButtonDown(0);
        isRightButtonClicked = Input.GetMouseButtonDown(1);
    }

    private void LeftMouseBtnClick()
    {
        if (isLeftButtonClicked)
        {
            if (!GrabGun())
            {
                HitMotion();
                ShotGunMotion();
            }
        }
        else
        {
            string hitType = punched ? "Hit1" : "Hit2";
            animator.SetBool(hitType, false);
        }
    }

    private bool GrabGun()
    {
        if (!isHolding)
        {
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), mainCamera.transform.forward);
            float searchRange = 0.5f;
            
            if (Physics.SphereCast(ray, searchRange, out RaycastHit sphereHit, shotRayLength))
            {
                if (sphereHit.transform.TryGetComponent(out Gun gun) && !gun.transform.root.CompareTag("Enemy"))
                {
                    holdingObj = sphereHit.transform.gameObject;
                    gun.HoldedToHand();
                }
            }
            if (Physics.Raycast(ray, out RaycastHit rayHit, shotRayLength))
            {
                if (rayHit.transform.TryGetComponent(out Gun gun) && !gun.transform.root.CompareTag("Enemy"))
                {
                    holdingObj = rayHit.transform.gameObject;
                    gun.HoldedToHand();
                }
            }
            if (holdingObj != null)
            {
                playerState.holdingObj = holdingObj;
                return true;
            }
        }
        return false;
    }

    private void HitMotion()
    {
        string hitType = !punched ? "Hit1" : "Hit2";

        if (!isHolding)
        {
            animator.SetBool(hitType, true);
            StartCoroutine(InputNearHeatDamage(hitType));
        }
    }

    private void CheckPunchType()
    {
        var animation = animator.GetCurrentAnimatorStateInfo(0);

        if (animation.IsTag("Act"))
        {
            punched = animation.IsName("Hit1");
        }
    }

    IEnumerator InputNearHeatDamage(string hitType)
    {
        float actionEndTiming = 0.9f;

        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(hitType))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > actionEndTiming)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, nearAttackLength) && hit.transform.CompareTag("Enemy"))
                    {
                        EnemyLife _EnemyLife = hit.transform.gameObject.GetComponent<EnemyLife>();
                        _EnemyLife.isDead = true;
                        _EnemyLife.deadByGun = false;
                    }
                    break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void ShotGunMotion()
    {
        if (!isActing && holdingObj != null)
        {
            if (holdingObj.TryGetComponent(out Gun gun) && !gun.isCatching)
            {
                animator.SetBool("Shot", true);
                gun.ShotGun();
            }
        }
    }

    private void ThrowMotion()
    {
        if (isRightButtonClicked && holdingObj != null)
        {
            if (holdingObj.TryGetComponent(out Gun gun))
            {
                if (!isActing && !gun.isCatching)
                {
                    animator.SetBool("Throw", true);
                    gun.Throwing();
                }
            }
        }
        else
        {
            animator.SetBool("Throw", false);
        }
    }
}
