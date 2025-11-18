using UnityEngine;

public class TroopSpawner : MonoBehaviour
{
    public GameObject archerPrefab;
    public Transform spawnPoint;

    public void SpawnArcher()
    {
        Instantiate(archerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
