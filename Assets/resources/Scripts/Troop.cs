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
    public float attackRange = 3f;         // Melee: small | Ranged: bigger
    public float attackCooldown = 1f;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;    // Only used if troopType == Ranged
    public Transform projectileSpawnPoint; // Where the arrow comes from
    public float projectileSpeed = 15f;

    [Header("Targeting")]
    public float scanInterval = 0.3f;      // How often to look for enemies
    public float baseAttackRange = 2.5f;   // Distance to base to start attacking

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

        // Decide which base to attack based on team
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
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        scanTimer -= Time.deltaTime;

        // Regularly scan for closest enemy troop
        if (scanTimer <= 0f)
        {
            scanTimer = scanInterval;
            FindClosestEnemyTroop();
        }

        // If we have an enemy target in range → fight troop
        if (currentEnemyTarget != null)
        {
            float distToEnemy = Vector3.Distance(transform.position, currentEnemyTarget.transform.position);

            if (distToEnemy <= attackRange + 0.2f) // small tolerance
            {
                agent.isStopped = true;
                transform.LookAt(currentEnemyTarget.transform.position);
                TryAttackTroop();
                return;
            }
        }

        // No enemy troop in range → move toward base
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

            if (troop == null || troop.teamComponent == null || teamComponent == null)
                continue;

            if (troop.teamComponent.team == teamComponent.team)
                continue;

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
        if (currentEnemyTarget == null) return;
        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        if (troopType == TroopType.Melee)
        {
            // Direct damage
            currentEnemyTarget.TakeDamage(attackDamage);
        }
        else if (troopType == TroopType.Ranged)
        {
            FireProjectileAt(currentEnemyTarget.transform);
        }
    }

    private void TryAttackBase()
    {
        if (targetBase == null) return;
        if (attackTimer > 0f) return;

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
        if (projectilePrefab == null) return;

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
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
