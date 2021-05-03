using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionChecker : StateMachineBehaviour
{
    private PlayerState playerState;

    private void Awake()
    {
        playerState = PlayerState.GetInstance();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("Act"))
        {
            if (stateInfo.normalizedTime <= 0.95f)
            {
                playerState.isActing = true;
            }
            else
            {
                playerState.isActing = false;
            }
        }
        else if (stateInfo.IsName("Walk"))
        {
            playerState.isActing = true;
        }
        else if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            playerState.isActing = true;
        }
        else
        {
            playerState.isActing = false;
        }
    }
}
