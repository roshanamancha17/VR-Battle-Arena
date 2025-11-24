using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnButtonCooldown : MonoBehaviour
{
    public Image cooldownOverlay;
    public float cooldownTime = 3f;
    private bool isCoolingDown = false;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        cooldownOverlay.fillAmount = 0f; // hidden initially
    }

    public void TriggerCooldown()
    {
        if (!isCoolingDown)
            StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        button.interactable = false;

        float elapsed = 0f;
        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownOverlay.fillAmount = elapsed / cooldownTime;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        button.interactable = true;
        isCoolingDown = false;
    }
}
