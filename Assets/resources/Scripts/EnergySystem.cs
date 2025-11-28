using UnityEngine;
using UnityEngine.Events;

public class EnergySystem : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 10f;
    public float regenRate = 1f;   // units per second
    public float currentEnergy = 0f;

    // Broadcast event when energy changes (UI listens to this)
    public UnityEvent<float, float> onEnergyChanged;

    private void Start()
    {
        currentEnergy = maxEnergy;   // start full, change to 0 if needed
        onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += regenRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

            onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }
    }

    public bool TrySpend(float cost)
    {
        if (currentEnergy < cost) return false;

        currentEnergy -= cost;
        onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        return true;
    }
}
