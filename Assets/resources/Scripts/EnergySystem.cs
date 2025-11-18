using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 10f;
    public float rechargeRate = 1f;
    public float currentEnergy = 0f;

    [Header("UI")]
    public Slider energySlider;

    void Start()
    {
        currentEnergy = 0;
        if (energySlider != null)
            energySlider.maxValue = maxEnergy;
    }

    void Update()
    {
        Recharge();
        UpdateUI();
    }

    void Recharge()
    {
        currentEnergy += rechargeRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    void UpdateUI()
    {
        if (energySlider != null)
            energySlider.value = currentEnergy;
    }

    public bool TrySpend(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }
        return false;
    }
}
