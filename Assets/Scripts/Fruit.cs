using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Fruit : MonoBehaviour
{
    public float Nutrition
    {
        get { return profile.nutrition; }
    }

    public float ripeness;
    public bool isDetached;

    [HideInInspector] public FruitTree parent;
    [HideInInspector] public Transform spawnPoint;
    
    [SerializeField] private PlantProfile profile;

    private Rigidbody rb;
    private CoroutineHandle growthHandle;
    private Renderer meshRenderer;

    private static float framesPerSecond = 60;
    private static float maxRipeness = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<Renderer>();
        ripeness = 0;
        meshRenderer.material.SetColor("_Color", profile.initialColor);
    }

    private void OnEnable()
    {
        growthHandle = Timing.RunCoroutine(GrowthCycle());
    }
    private void OnDisable()
    {
        Timing.PauseCoroutines(growthHandle);
    }
    public bool CheckEdibility(EntityTypes type)
    {
        return profile.edibilityList.Contains(type);
    }
    private void Detach()
    {
        rb.isKinematic = false;
        parent.FreeSpot(spawnPoint);
    }

    IEnumerator<float> GrowthCycle()
    {
        while (true)
        {
            ripeness += profile.growthRate / framesPerSecond;
            if (ripeness >= maxRipeness && !isDetached)
            {
                Detach();
                isDetached = true;
                meshRenderer.material.SetColor("_Color", profile.matureColor);
            }
            if (ripeness >= profile.decayThreshold) Destroy(this.gameObject);

            yield return Timing.WaitForOneFrame;
        }
    }
}
