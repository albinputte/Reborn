using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, float damageAmount)
    {

        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.6f), 0);
        Vector3 spawnPosition = position + randomOffset;

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, spawnPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUp(damageAmount);

        return damagePopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 0.5f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(float damageAmount)
    {
        textMesh.SetText(damageAmount % 1 == 0 ? damageAmount.ToString("0") : damageAmount.ToString("0.0"));
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0.3f, 0.7f) * 5f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;
        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            float increaseScaleAmount = 0.5f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 0.5f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 1f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Other popups:

  

    public static DamagePopup CreateForHeal(Vector3 position, float healAmount)
    {

        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.6f), 0);
        Vector3 spawnPosition = position + randomOffset;

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, spawnPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUpHealPopup(healAmount);

        return damagePopup;
    }
    public void SetUpHealPopup(float healAmount)
    {
        textMesh.SetText(healAmount % 1 == 0 ? $"+{healAmount.ToString("0")}" : $"+{healAmount.ToString("0.0")}");
        textColor = new Color(44f / 255f, 159f / 255f, 76f / 255f);
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0.3f, 0.7f) * 10f;
    }

    public static DamagePopup CreateForPlayer(Vector3 position, float damageAmount)
    {

        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.4f), 0);
        Vector3 spawnPosition = position + randomOffset;

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, spawnPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUpPlayerDamagePopup(damageAmount);

        return damagePopup;
    }
    public void SetUpPlayerDamagePopup(float damageAmount)
    {
        textMesh.SetText(damageAmount % 1 == 0 ? damageAmount.ToString("0") : damageAmount.ToString("0.0"));

        // Use #ac2847
        textColor = new Color(123f / 255f, 37f / 255f, 54f / 255f);

        textMesh.color = textColor;

        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        // Slightly larger movement
        moveVector = new Vector3(0.3f, 0.4f) * 8f;

        // Optional: start slightly bigger
        transform.localScale = Vector3.one * 1.8f;
    }
    public static DamagePopup CreateForPlayerHeal(Vector3 position, float healAmount)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.6f), 0);
        Vector3 spawnPosition = position + randomOffset;

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, spawnPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUpPlayerHealPopup(healAmount);

        return damagePopup;
    }
    public void SetUpPlayerHealPopup(float healAmount)
    {
        float rounded = Mathf.Round(healAmount * 10f) / 10f;
        textMesh.SetText(rounded % 1 == 0 ? $"+{rounded.ToString("0")}" : $"+{rounded.ToString("0.0")}");


        textColor = new Color(44f / 255f, 159f / 255f, 76f / 255f);


        textMesh.color = textColor;

        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        // Use same movement as normal heal
        moveVector = new Vector3(0.3f, 0.7f) * 10f;

        // Match size with player damage popup
        transform.localScale = Vector3.one * 1.5f;
    }


    //crit:
    public static DamagePopup CreateForCrit(Vector3 position, float damageAmount)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.6f), 0);
        Vector3 spawnPosition = position + randomOffset;

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, spawnPosition, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUpCritPopup(damageAmount);

        return damagePopup;
    }

    public void SetUpCritPopup(float damageAmount)
    {
        textMesh.SetText(damageAmount % 1 == 0 ? damageAmount.ToString("0") : damageAmount.ToString("0.0"));

        textColor = new Color(216f / 255f, 95f / 255f, 40f / 255f);

        textMesh.color = textColor;

        disappearTimer = DISAPPEAR_TIMER_MAX + 0.4f; // last longer than normal
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        // Larger movement and scale
        moveVector = new Vector3(0.3f, 0.8f) * 10f;
        transform.localScale = Vector3.one * 2f;
    }
    // poison





}
