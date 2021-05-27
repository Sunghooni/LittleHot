using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GameObject brokenEffect;
    public GameObject camera;

    public void DeadMotion()
    {
        PlayerState.GetInstance().isActing = false;
        PlayerState.GetInstance().isRotate = false;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        brokenEffect.SetActive(true);
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        brokenEffect.transform.SetParent(null);
        camera.transform.SetParent(null);

        camera.AddComponent<BoxCollider>();
        camera.AddComponent<Rigidbody>();

        gameObject.SetActive(false);

        if (PlayerState.GetInstance().isHolding)
        {
            Gun gun = PlayerState.GetInstance().holdingObj.GetComponent<Gun>();
            gun.isHolded = false;
            gun.ThrowGunByAttack();
        }
    }
}
