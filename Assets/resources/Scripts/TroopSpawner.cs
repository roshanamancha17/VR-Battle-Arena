using UnityEngine;

public class TroopSpawner : MonoBehaviour
{
    public GameObject archerPrefab;
    public Transform spawnPoint;
    public SpawnButtonCooldown cooldownButton;


    public void SpawnArcher()
    {
        if (cooldownButton != null)
        cooldownButton.TriggerCooldown();

    Instantiate(archerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
