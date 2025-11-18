using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    public float damage = 10f;
    private NavMeshAgent agent;
    private BaseHealth targetBase;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Find enemy base
        targetBase = GameObject.FindWithTag("EnemyBase").GetComponent<BaseHealth>();

        // Set destination
        agent.SetDestination(targetBase.transform.position);
    }

    private void Update()
    {
        // When close enough, do damage
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            targetBase.TakeDamage(damage);
            Destroy(gameObject); // Kill troop after attack
        }
    }
}
