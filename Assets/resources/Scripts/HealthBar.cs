using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Image fillImage;

    void Start()
    {
        if (slider == null) slider = GetComponentInChildren<Slider>();
        fillImage = slider.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    public void SetHealth(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    public void SetColor(Color color)
    {
        if (fillImage != null)
            fillImage.color = color;
    }
}
