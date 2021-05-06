using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEffects : MonoBehaviour
{
    public GameObject brokenEffect;
    public GameObject[] bodyParts;
    private readonly float delayTime = 0.3f;

    public void ShowBrokenParts()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        float headFixedPosition = -0.1f;
        float headFixedRotation = 180;

        parent.transform.GetChild(0).gameObject.SetActive(false);
        parent.GetComponent<CapsuleCollider>().isTrigger = true;
        parent.GetComponent<Rigidbody>().isKinematic = true;

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

        Invoke(nameof(SetPartsApart), delayTime);
    }

    private void SetPartsApart()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            //To make parts free from avatar/animation
            bodyParts[i].transform.SetParent(gameObject.transform.parent);
        }
    }
}
