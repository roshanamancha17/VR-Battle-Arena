using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 500f;
    public float currentHealth;

    [Header("Team")]
    public Team team; // Player or Enemy

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnBaseDestroyed();
        }
    }

    private void OnBaseDestroyed()
    {
        Debug.Log(name + " base destroyed!");

        bool isPlayer = (team == Team.Player);
        GameManager.Instance.OnBaseDestroyed(isPlayer);
    }
}
