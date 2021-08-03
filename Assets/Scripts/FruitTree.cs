using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;

public class FruitTree : MonoBehaviour
{
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private float respawnRate;
    [SerializeField] private float respawnChance;    

    private Dictionary<Transform, bool> spawnPoints = new Dictionary<Transform, bool>();
    private List<Transform> valuesToChange = new List<Transform>();
    private CoroutineHandle respawnHandle;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child, true);
        }
    }
    private void OnEnable()
    {
        respawnHandle = Timing.RunCoroutine(RespawnCycle());
    }
    private void OnDisable()
    {
        Timing.PauseCoroutines(respawnHandle);
    }

    public void FreeSpot(Transform spawnPoint)
    {
        spawnPoints[spawnPoint] = true;
    }

    IEnumerator<float> RespawnCycle()
    {
        while (true)
        {
            valuesToChange.Clear();

            foreach (KeyValuePair<Transform, bool> entry in spawnPoints) //TODO
            {
                if (entry.Value == true)
                {
                    if (!RandomSpawn()) continue;
                    Fruit _newFruit = Instantiate(fruitPrefab, entry.Key.position, Quaternion.identity).GetComponent<Fruit>();
                    _newFruit.parent = this;
                    _newFruit.spawnPoint = entry.Key;
                    valuesToChange.Add(entry.Key);
                }
            }

            valuesToChange.ForEach(x => spawnPoints[x] = false);

            yield return Timing.WaitForSeconds(respawnRate);
        }
    }

    private bool RandomSpawn()
    {
        int _random = Random.Range(0, 100);
 
        return _random < respawnChance;
    }
}
