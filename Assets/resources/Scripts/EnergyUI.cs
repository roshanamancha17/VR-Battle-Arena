using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Slider energySlider;

    public void UpdateEnergy(float current, float max)
    {
        energySlider.value = current / max;
    }
}
