using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCreatureProfile", menuName = "ScriptableObjects/CreatureProfile", order = 2)]

public class CreatureProfile : ScriptableObject
{
    [Tooltip("Hunger drop per second")]
    public float hungerRate;
    [Tooltip("Size of the trigger used for detecting entities")]
    public float perceptionRange;
    [Tooltip("Max distance for choosing random waypoints")]
    public float wanderMaxRange;
    [Tooltip("Min distance for choosing random waypoints")]
    public float wanderMinRange;
    [Tooltip("Distance for interaction with other entities")]
    public float interactionRange;

    //other needs

    //diet

    //other
}
