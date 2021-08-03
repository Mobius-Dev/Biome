using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlantProfile", menuName = "ScriptableObjects/PlantProfile", order = 3)]

public class PlantProfile : ScriptableObject
{
    [Tooltip("Growth/Ripeness change per second (100 is fully grown)")]
    public float growthRate;
    [Tooltip("Ripeness level for fruit to decay (gameobject destroyed")]
    public float decayThreshold;    
    [Tooltip("Color after spawning")]
    public Color initialColor;
    [Tooltip("Fully grown/mature color")]
    public Color matureColor;
    [Tooltip("Amount of nutrition to be added when eaten")]
    public float nutrition;
    [Tooltip("Determines kind of creature can eat this")]
    public List<EntityTypes> edibilityList;
}