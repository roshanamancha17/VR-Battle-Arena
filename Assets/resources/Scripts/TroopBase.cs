using UnityEngine;
using UnityEngine.AI;

public class TroopBase : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public float attackDamage = 10;
    public float attackRate = 1f;
    public float attackRange = 2f;

    public float attackCooldown = 0f;

    private NavMeshAgent agent;
    private GameObject target;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        MoveOrAttack();
    }

    void FindTarget()
    {
        // Find enemy base first
        target = GameObject.FindGameObjectWithTag("EnemyBase");

        // Later: Find closest enemy troop instead
    }

    void MoveOrAttack()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > attackRange)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.ResetPath();
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        attackCooldown = attackRate;

        BaseHealth baseHP = target.GetComponent<BaseHealth>();
        if (baseHP != null)
            baseHP.TakeDamage(attackDamage);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
