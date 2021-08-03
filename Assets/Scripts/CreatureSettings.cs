using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCreatureSettings", menuName = "ScriptableObjects/CreatureSettings", order = 1)]

public class CreatureSettings : ScriptableObject
{
    public float decisionCycleRate;
    //maybe remove this if no more things to be put here
}
