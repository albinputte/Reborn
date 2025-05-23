using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenBehind : MonoBehaviour
{
    public float transparentAlpha = 0.3f;
    public float fadeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Coroutine fadeCoroutine;
    private bool isTransparent = false;
    private bool playerInside = false;
    private Transform player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!playerInside || player == null)
        {
            if (isTransparent)
            {
                StartFade(1f);
                isTransparent = false;
            }
            return;
        }

        if (player.position.y > transform.position.y && !isTransparent)
        {
            StartFade(transparentAlpha);
            isTransparent = true;
        }
        else if (player.position.y <= transform.position.y && isTransparent)
        {
            StartFade(1f);
            isTransparent = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
