using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Transform target;
    private Troop shooter;
    private bool hasHit = false;

    public void Initialize(Transform enemy, Troop shooterTroop)
    {
        target = enemy;
        shooter = shooterTroop;

        if (target != null)
            transform.LookAt(target.position);

        Destroy(gameObject, lifetime); // auto clean
    }

    private void Update()
    {
        if (hasHit) return;

        // If target disappeared while flying → destroy arrow
        if (target == null || shooter == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        // If reached near target → hit
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        hasHit = true;

        // Damage troop
        Troop troop = target.GetComponent<Troop>();
        if (troop != null && troop != shooter)
            troop.TakeDamage(damage);

        // Damage base
        BaseHealth baseHealth = target.GetComponent<BaseHealth>();
        if (baseHealth != null)
            baseHealth.TakeDamage(damage);

        Destroy(gameObject); // remove arrow immediately
    }
}
