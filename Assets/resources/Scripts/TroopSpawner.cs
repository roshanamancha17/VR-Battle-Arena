using UnityEngine;

public class TroopSpawner : MonoBehaviour
{
    [Header("Troop Prefabs")]
    public GameObject knightPrefab;
    public GameObject archerPrefab;
    public GameObject tankPrefab;

    [Header("Spawn Points")]
    public Transform knightSpawnPoint;
    public Transform archerSpawnPoint;
    public Transform tankSpawnPoint;

    [Header("Energy System")]
    public EnergySystem energySystem;

    [Header("Costs")]
    public float knightCost = 3f;
    public float archerCost = 5f;
    public float tankCost = 7f;

    [Header("Cooldown Buttons")]
    public SpawnButtonCooldown knightButtonCooldown;
    public SpawnButtonCooldown archerButtonCooldown;
    public SpawnButtonCooldown tankButtonCooldown;

    public void SpawnKnight()
    {
        if (!energySystem.TrySpend(knightCost)) return;

        Instantiate(knightPrefab, knightSpawnPoint.position, knightSpawnPoint.rotation);
        knightButtonCooldown?.TriggerCooldown();
    }

    public void SpawnArcher()
    {
        if (!energySystem.TrySpend(archerCost)) return;

        Instantiate(archerPrefab, archerSpawnPoint.position, archerSpawnPoint.rotation);
        archerButtonCooldown?.TriggerCooldown();
    }

    public void SpawnTank()
    {
        if (!energySystem.TrySpend(tankCost)) return;

        Instantiate(tankPrefab, tankSpawnPoint.position, tankSpawnPoint.rotation);
        tankButtonCooldown?.TriggerCooldown();
    }
}
