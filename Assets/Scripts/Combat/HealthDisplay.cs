using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] GameObject healthBarParent = null;
    [SerializeField] Image healthBarImage = null;

    private Coroutine coroutineWaitHealthBar = null;

    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
        health.ShowHealthBar += ActivateHealthParent;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
        health.ShowHealthBar -= ActivateHealthParent;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    private void ActivateHealthParent()
    {
        healthBarParent.SetActive(true);

        if (coroutineWaitHealthBar != null)
            StopCoroutine(coroutineWaitHealthBar);

        coroutineWaitHealthBar = StartCoroutine(HideHealthBar());
    }

    private IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(4f);
        healthBarParent.SetActive(false);
    }

    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth;
    }
}
