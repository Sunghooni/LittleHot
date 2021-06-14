using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public PlayModeSO playModeSO;

    private void Awake()
    {
        Setting();
    }

    private void Setting()
    {
        //Set ReplayMode Off
        playModeSO.isReplayMode = false;

        //Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
