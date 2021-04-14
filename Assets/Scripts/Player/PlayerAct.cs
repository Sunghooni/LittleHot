using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    public MouseCtrl mouseCtrl;

    private PlayerState playerState;
    private Animator animator;
    private GameObject holdingObj;
    private bool isActing;
    private bool isHolding;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        playerState = PlayerState.GetInstance();
    }

    private void Update()
    {
        holdingObj = playerState.holdingObj;
        isActing = playerState.isActing;
        isHolding = playerState.isHolding;

        ThrowMotion();
        LeftMouseBtnClick();
    }

    private void LeftMouseBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GrabGun())
            {
                HitMotion();
                ShotGunMotion();
            }
        }
        else
        {
            animator.SetBool("Hit1", false);
            animator.SetBool("Hit2", false);
        }
    }

    private bool GrabGun()
    {
        if (!isHolding)
        {
            holdingObj = mouseCtrl.ShotRay();

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
        string hitType = Random.Range(0, 2) == 0 ? "Hit1" : "Hit2";

        if (!isHolding)
        {
            animator.SetBool(hitType, true);
        }
    }

    private void ShotGunMotion()
    {
        if (!isActing && holdingObj != null)
        {
            if (holdingObj.TryGetComponent(out Gun gun))
            {
                animator.SetBool("Shot", true);
                gun.ShotGun();
            }
        }
    }

    private void ThrowMotion()
    {
        if (Input.GetMouseButtonDown(1) && holdingObj != null)
        {
            animator.SetBool("Throw", true);

            if (holdingObj.TryGetComponent(out Gun gun) && !isActing) //총을 들고 있을 경우 실행
            {
                gun.ThrowMotion();
            }
        }
        else
        {
            animator.SetBool("Throw", false);
        }
    }
}
