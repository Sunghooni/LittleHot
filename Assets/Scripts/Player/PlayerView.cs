using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject Camera;

    private Transform handPos;
    private Animator animator;
    private bool releaseIK = false;

    private float mouseY;
    private float cameraX = 0;
    private const float rotSpeed = 2;

    public float CameraX
    {
        get
        {
            cameraX = cameraX <= 0 ? cameraX + 360 : cameraX;
            cameraX = cameraX > 360 ? cameraX - 360 : cameraX;

            if (cameraX > 30 && cameraX < 180)
            {
                cameraX = 30;
            }
            else if (cameraX < 330 && cameraX > 180)
            {
                cameraX = 330;
            }
            return cameraX;
        }
        set
        {
            cameraX = value;
        }
    }

    private void Awake()
    {
        animator = PlayerState.GetInstance().animator;
        handPos = PlayerState.GetInstance().handAimPos.transform;
    }

    private void Update()
    {
        mouseY = Input.GetAxisRaw("Mouse Y");
        CheckActing();
    }

    private void FixedUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        var cameraRot = gameObject.transform.eulerAngles;
        CameraX += -mouseY * rotSpeed;

        Camera.transform.eulerAngles = new Vector3(CameraX, cameraRot.y, cameraRot.z);
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