using UnityEngine;
using UnityEngine.AI;

public class TroopSpawnerVR : MonoBehaviour
{
    public EnergySystem energySystem;

    [Header("Costs")]
    public float knightCost = 3f;
    public float archerCost = 5f;
    public float tankCost = 7f;

    [Header("Prefabs")]
    public GameObject knightPrefab;
    public GameObject archerPrefab;
    public GameObject tankPrefab;

    [Header("Spawn Points")]
    public Transform knightSpawn;
    public Transform archerSpawn;
    public Transform tankSpawn;

    public void SpawnKnight()
    {
        if (!energySystem.TrySpend(knightCost)) return;
        Spawn(knightPrefab, knightSpawn);
    }

    public void SpawnArcher()
    {
        if (!energySystem.TrySpend(archerCost)) return;
        Spawn(archerPrefab, archerSpawn);
    }

    public void SpawnTank()
    {
        if (!energySystem.TrySpend(tankCost)) return;
        Spawn(tankPrefab, tankSpawn);
    }

    void Spawn(GameObject prefab, Transform point)
    {
        GameObject troop = Instantiate(prefab, point.position, point.rotation);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point.position, out hit, 5f, NavMesh.AllAreas))
            troop.transform.position = hit.position;
    }
}
