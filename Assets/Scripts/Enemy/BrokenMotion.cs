using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMotion : MonoBehaviour
{
    public PlayModeSO playModeSO;
    public GameObject bodyParts;
    private Animator animator;
    private EnemyLife _EnemyLife;

    private bool isMotionplayed = false;
    private float leftHandMovement = 3f;
    private float rightHandMovement = 3f;
    private const float spineRotateX = 45f;

    private void Awake()
    {
        animator = gameObject.transform.GetComponent<Animator>();
        _EnemyLife = gameObject.transform.GetComponent<EnemyLife>();
    }

    private void Start()
    {
        float fixedMovement = -2f;
        float randomValue = Random.Range(leftHandMovement + fixedMovement, leftHandMovement);

        leftHandMovement = randomValue;
        rightHandMovement = randomValue;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_EnemyLife.isDead)
        {
            Vector3 leftHandPos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            Vector3 rightHandPos = animator.GetIKPosition(AvatarIKGoal.RightHand);
            Vector3 spineRotation = animator.GetBoneTransform(HumanBodyBones.Spine).eulerAngles;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

            leftHandPos += transform.up * leftHandMovement;
            rightHandPos += transform.up * rightHandMovement;
            spineRotation += transform.right * spineRotateX;

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
            animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(spineRotation));

            if (!isMotionplayed)
            {
                bodyParts.GetComponent<BrokenEffects>().ShowBrokenParts();

                if (!playModeSO.isReplayMode)
                {
                    AudioManager.instance.PlaySound("BrokenSFX", gameObject);
                }
            }
            isMotionplayed = true;
        }
    }
}
