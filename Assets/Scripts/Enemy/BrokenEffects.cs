using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEffects : MonoBehaviour
{
    public GameObject holdingGun;
    public GameObject brokenEffect;
    public GameObject[] bodyParts;
    private readonly float partsDelay = 0.3f;
    private readonly float gunThrowPower = 5f;

    public void ShowBrokenParts()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        float headFixedPosition = -0.1f;
        float headFixedRotation = 180;

        parent.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(parent.GetComponent<CapsuleCollider>());

        //Particle Play
        brokenEffect.SetActive(true);

        //Unity Plysics can't act correctly in changed TimeScale/fixedDeltaTime
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (bodyParts[i].transform.name.Equals("Head"))
            {
                bodyParts[i].transform.position += transform.forward * headFixedPosition;
                bodyParts[i].transform.eulerAngles += transform.up * headFixedRotation;
            }
            bodyParts[i].SetActive(true);
        }

        ThrowGunByAttack();
        Invoke(nameof(SetPartsApart), partsDelay);
    }

    private void SetPartsApart()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            //To make parts free from avatar/animation
            bodyParts[i].transform.SetParent(gameObject.transform.parent);
        }

        StartCoroutine(SetPartsDisappear());
    }

    private void ThrowGunByAttack()
    {
        Rigidbody holdingGunRigid = holdingGun.transform.GetComponent<Rigidbody>();
        holdingGunRigid.useGravity = true;
        holdingGunRigid.isKinematic = false;
        holdingGun.transform.SetParent(null);

        holdingGunRigid.AddForce(transform.up * gunThrowPower, ForceMode.Impulse);
    }

    IEnumerator SetPartsDisappear()
    {
        float disappearDelay = 1f;
        yield return new WaitForSeconds(disappearDelay);

        for (int i = 0; i < bodyParts.Length; i++)
        {
            //To make parts free from avatar/animation
            bodyParts[i].transform.GetComponent<MeshCollider>().isTrigger = true;
        }

        yield return new WaitForSeconds(partsDelay);

        Destroy(gameObject.transform.root.gameObject);
    }
}
