using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLavaDamage : MonoBehaviour
{
    public int damagePerTick;
    public int amountOfTick;
    public float tickInterval;
    public float timeToStartFading;
    public float fadeDuration = 1f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TimerUntilStartFading(timeToStartFading));
    }

    public IEnumerator TimerUntilStartFading(float timeToStartFading)
    {
        yield return new WaitForSeconds(timeToStartFading);
        yield return StartCoroutine(FadeOut());
        Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;
        if(collision.gameObject.CompareTag("Player"))
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();
            if (damagable != null)
            {
                damagable.ApplyDamageOverTime(damagePerTick, tickInterval, amountOfTick);
            }
        }
    }
}

     