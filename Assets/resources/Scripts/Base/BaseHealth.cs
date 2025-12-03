using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth;
    public Team team;

    private void Start()
    {
        currentHealth = maxHealth;

        // Initialize UI once
        if (team == Team.Player)
            BaseHealthUIManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
        else
            BaseHealthUIManager.Instance.UpdateEnemyHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update UI only when damaged
        if (team == Team.Player)
            BaseHealthUIManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
        else
            BaseHealthUIManager.Instance.UpdateEnemyHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            GameManagerVR.Instance.OnBaseDestroyed(team == Team.Player);
        }
    }
}
