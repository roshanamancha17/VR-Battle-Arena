using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TeamComponent))]
public class Troop : MonoBehaviour
{
    [Header("Troop Type")]
    public TroopType troopType = TroopType.Melee;

    [Header("Combat Stats")]
    public float maxHealth = 50f;
    public float attackDamage = 10f;
    public float attackRange = 3f;
    public float attackCooldown = 1f;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 15f;

    [Header("Health Bar")]
    public GameObject playerHealthBarPrefab;
    public GameObject enemyHealthBarPrefab;
    private HealthBar healthBar;

    [Header("Targeting")]
    public float scanInterval = 0.3f;
    public float baseAttackRange = 2.5f;

    private float currentHealth;
    private float attackTimer = 0f;
    private float scanTimer = 0f;

    private NavMeshAgent agent;
    private TeamComponent teamComponent;
    private BaseHealth targetBase;
    private Troop currentEnemyTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        teamComponent = GetComponent<TeamComponent>();
        currentHealth = maxHealth;

        // Assign the correct enemy base
        if (teamComponent.team == Team.Player)
        {
            var enemyBaseObj = GameObject.FindWithTag("EnemyBase");
            if (enemyBaseObj != null)
                targetBase = enemyBaseObj.GetComponent<BaseHealth>();
        }
        else
        {
            var playerBaseObj = GameObject.FindWithTag("PlayerBase");
            if (playerBaseObj != null)
                targetBase = playerBaseObj.GetComponent<BaseHealth>();
        }

        if (targetBase != null)
            agent.SetDestination(targetBase.transform.position);

        // ---------- Spawn Health Bar ----------
        GameObject canvasObj = GameObject.FindGameObjectWithTag("WorldCanvas");
        if (canvasObj != null)
        {
            GameObject prefabToUse = (teamComponent.team == Team.Player)
                ? playerHealthBarPrefab
                : enemyHealthBarPrefab;

            GameObject hb = Instantiate(prefabToUse, transform.position + Vector3.up * 2f, Quaternion.identity, canvasObj.transform);

            healthBar = hb.GetComponent<HealthBar>();
            healthBar.target = transform;
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        scanTimer -= Time.deltaTime;

        // Scan nearest enemy troop
        if (scanTimer <= 0f)
        {
            scanTimer = scanInterval;
            FindClosestEnemyTroop();
        }

        // If fighting another troop
        if (currentEnemyTarget != null)
        {
            float distToEnemy = Vector3.Distance(transform.position, currentEnemyTarget.transform.position);

            if (distToEnemy <= attackRange + 0.2f)
            {
                agent.isStopped = true;
                transform.LookAt(currentEnemyTarget.transform.position);
                TryAttackTroop();
                return;
            }
        }

        // Move towards base if no troop target
        agent.isStopped = false;
        if (targetBase != null)
        {
            agent.SetDestination(targetBase.transform.position);

            float distToBase = Vector3.Distance(transform.position, targetBase.transform.position);
            if (distToBase <= baseAttackRange)
            {
                agent.isStopped = true;
                transform.LookAt(targetBase.transform.position);
                TryAttackBase();
            }
        }
    }

    private void FindClosestEnemyTroop()
    {
        Troop[] allTroops = Object.FindObjectsByType<Troop>(FindObjectsSortMode.None);
        float closestDist = Mathf.Infinity;
        currentEnemyTarget = null;

        foreach (var troop in allTroops)
        {
            if (troop == this) continue;
            if (troop == null || troop.teamComponent == null || teamComponent == null) continue;
            if (troop.teamComponent.team == teamComponent.team) continue;

            float dist = Vector3.Distance(transform.position, troop.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                currentEnemyTarget = troop;
            }
        }
    }

    private void TryAttackTroop()
    {
        if (currentEnemyTarget == null || attackTimer > 0f) return;

        attackTimer = attackCooldown;

        if (troopType == TroopType.Melee)
        {
            currentEnemyTarget.TakeDamage(attackDamage);
        }
        else if (troopType == TroopType.Ranged)
        {
            FireProjectileAt(currentEnemyTarget.transform);
        }
    }

    private void TryAttackBase()
    {
        if (targetBase == null || attackTimer > 0f) return;

        attackTimer = attackCooldown;

        if (troopType == TroopType.Melee)
        {
            targetBase.TakeDamage(attackDamage);
        }
        else if (troopType == TroopType.Ranged)
        {
            FireProjectileAt(targetBase.transform);
        }
    }

    private void FireProjectileAt(Transform target)
    {
        if (!projectilePrefab) return;

        Vector3 spawnPos = projectileSpawnPoint != null
            ? projectileSpawnPoint.position
            : transform.position + transform.forward * 0.5f;

        GameObject projObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projectile = projObj.GetComponent<Projectile>();
        projectile.damage = attackDamage;
        projectile.speed = projectileSpeed;
        projectile.Initialize(target, this);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        Destroy(gameObject);
    }
}
