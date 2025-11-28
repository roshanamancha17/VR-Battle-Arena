using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 500f;
    public float currentHealth;

    [Header("Team")]
    public Team team; // Player or Enemy

    [Header("Health Bar Prefabs")]
    public GameObject playerBaseHealthBarPrefab;
    public GameObject enemyBaseHealthBarPrefab;

    private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // ------- SPAWN HEALTH BAR --------
        GameObject canvasObj = GameObject.FindGameObjectWithTag("WorldCanvas");
        if (canvasObj == null)
        {
            Debug.LogError("❌ WorldCanvas not found! Please create a canvas and tag it as 'WorldCanvas'");
            return;
        }

        GameObject prefabToUse = (team == Team.Player)
            ? playerBaseHealthBarPrefab
            : enemyBaseHealthBarPrefab;

        if (prefabToUse == null)
        {
            Debug.LogError("❌ Missing BaseHealthBar Prefab on: " + gameObject.name);
            return;
        }

        GameObject hb = Instantiate(prefabToUse,
            transform.position + Vector3.up * 8f,
            Quaternion.identity,
            canvasObj.transform);

        healthBar = hb.GetComponent<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("❌ HealthBar script missing on prefab: " + prefabToUse.name);
            return;
        }

        healthBar.target = transform;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (healthBar != null)
                Destroy(healthBar.gameObject);

            OnBaseDestroyed();
        }
    }

    private void OnBaseDestroyed()
    {
        Debug.Log($"{name} base destroyed!");

        bool isPlayerBase = (team == Team.Player);
        GameManager.Instance.OnBaseDestroyed(isPlayerBase);
    }
}
