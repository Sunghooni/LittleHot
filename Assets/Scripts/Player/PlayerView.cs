using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Transform handPos;
    private Animator animator;
    private bool releaseIK = false;

    private void Awake()
    {
        animator = PlayerState.GetInstance().animator;
        handPos = PlayerState.GetInstance().handAimPos.transform;
    }

    private void Update()
    {
        CheckActing();
    }

    private void CheckActing()
    {
        PlayerState playerState = PlayerState.GetInstance();

        if (playerState.isHolding)
        {
            if(playerState.holdingObj.TryGetComponent(out Gun gun) && !gun.isThrowing)
            {
                releaseIK = false;
            }
            else
            {
                releaseIK = true;
            }
        }
        else
        {
            releaseIK = true;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!releaseIK)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            animator.SetIKPosition(AvatarIKGoal.RightHand, handPos.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, handPos.rotation);
        }
    }
}