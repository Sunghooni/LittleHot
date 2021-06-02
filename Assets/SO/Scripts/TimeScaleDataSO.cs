using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimeScaleDataSO : ScriptableObject
{
    public bool isReplaying = false;
    public List<float> timeScaleList = new List<float>();
}
