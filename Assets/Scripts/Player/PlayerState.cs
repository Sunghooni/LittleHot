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
}