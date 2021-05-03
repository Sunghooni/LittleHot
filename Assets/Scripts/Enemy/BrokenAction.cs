using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenAction : MonoBehaviour
{
    public void BrockenAction()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).CompareTag("EnemyBodyParts"))
            {
                GameObject[] objs = gameObject.transform.GetChild(i).GetComponent<BrockenParts>().bodyParts;

                for (int j = 0; j < objs.Length; j++)
                {
                    objs[j].SetActive(true);
                    objs[j].transform.SetParent(gameObject.transform);
                }
                break;
            }
        }
    }
}
