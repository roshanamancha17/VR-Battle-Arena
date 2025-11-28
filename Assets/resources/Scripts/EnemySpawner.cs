using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject archerPrefab;
    public GameObject knightPrefab;
    public GameObject tankPrefab;

    public Transform archerSpawnPoint;
    public Transform knightSpawnPoint;
    public Transform tankSpawnPoint;

    public float archerCooldown = 5f;
    public float knightCooldown = 7f;
    public float tankCooldown = 10f;

    private float archerTimer = 0f;
    private float knightTimer = 0f;
    private float tankTimer = 0f;

    void Update()
    {
        archerTimer += Time.deltaTime;
        knightTimer += Time.deltaTime;
        tankTimer += Time.deltaTime;

        if (archerTimer >= archerCooldown)
        {
            SpawnArcher();
            archerTimer = 0f;
        }

        if (knightTimer >= knightCooldown)
        {
            SpawnKnight();
            knightTimer = 0f;
        }

        if (tankTimer >= tankCooldown)
        {
            SpawnTank();
            tankTimer = 0f;
        }
    }

    void SpawnArcher()
    {
        Instantiate(archerPrefab, archerSpawnPoint.position, archerSpawnPoint.rotation);
    }

    void SpawnKnight()
    {
        Instantiate(knightPrefab, knightSpawnPoint.position, knightSpawnPoint.rotation);
    }

    void SpawnTank()
    {
        Instantiate(tankPrefab, tankSpawnPoint.position, tankSpawnPoint.rotation);
    }
}
