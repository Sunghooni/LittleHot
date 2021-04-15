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
    private readonly float shotRayLength = 100f;

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

        CheckPunchType();
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
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, shotRayLength))
            {
                if (hit.transform.TryGetComponent(out Gun gun))
                {
                    holdingObj = hit.transform.gameObject;
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
        if (Input.GetMouseButtonDown(1) && holdingObj != null)
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
