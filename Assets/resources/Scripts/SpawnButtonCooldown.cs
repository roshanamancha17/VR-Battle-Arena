using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnButtonCooldown : MonoBehaviour
{
    public float cooldownTime = 3f;
    public Image cooldownFill;

    private bool isCoolingDown = false;

    public void TriggerCooldown()
    {
        if (!isCoolingDown)
            StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        isCoolingDown = true;
        float timer = 0f;

        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;

            if (cooldownFill != null)
                cooldownFill.fillAmount = timer / cooldownTime;

            yield return null;
        }

        if (cooldownFill != null)
            cooldownFill.fillAmount = 0f;

        isCoolingDown = false;
    }
}
