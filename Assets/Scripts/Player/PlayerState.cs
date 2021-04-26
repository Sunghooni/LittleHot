using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject Player;
    public Animator animator;
    public GameObject handAimPos;
    public GameObject gunAimPos;
    public GameObject gunThrowPos;
    public GameObject holdingObj;
    public bool isActing;
    public bool isMotion;
    public bool isRotate;
    public bool isHolding;


    private static PlayerState instance;

    private void Awake()
    {
        instance = gameObject.GetComponent<PlayerState>();
    }

    public static PlayerState GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
        }

        return instance;
    }

    private void Update()
    {
        CheckActing();
    }

    private void CheckActing()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Act"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            {
                isActing = true;
            }
            else
            {
                isActing = false;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion"))
        {
            isActing = false;
        }
    }
}