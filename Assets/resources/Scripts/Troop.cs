using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    public float attackRange = 8f;
    public float shootCooldown = 1f;
    public GameObject projectilePrefab;

    private float cooldownTimer = 0f;
    private NavMeshAgent agent;
    private BaseHealth targetBase;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetBase = GameObject.FindWithTag("EnemyBase").GetComponent<BaseHealth>();
        agent.SetDestination(targetBase.transform.position);
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetBase.transform.position);

        // If within range, shoot
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            ShootAtTarget();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetBase.transform.position);
        }
    }

    void ShootAtTarget()
    {
        if (cooldownTimer > 0f)
            return;

        cooldownTimer = shootCooldown;

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, transform.position + transform.forward * 0.5f, Quaternion.identity);
        proj.GetComponent<  Projectile>().SetTarget(targetBase.transform);
    }
}
