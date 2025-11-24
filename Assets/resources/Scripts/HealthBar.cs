using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Slider slider;
    public Transform target;
    public Vector3 offset = new Vector3(0, 3f, 0);
    private Image fillImage;

    void Start()
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>();

        if (slider != null)
            fillImage = slider.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
        else
            Destroy(gameObject);
    }

    public void SetHealth(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    public void SetColorByTag(string tag)
    {
        if (tag.Contains("Player"))
            fillImage.color = Color.green;
        else if (tag.Contains("Enemy"))
            fillImage.color = Color.red;
    }
}
