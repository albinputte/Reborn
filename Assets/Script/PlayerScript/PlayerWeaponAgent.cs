using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponAgent : MonoBehaviour
{
    [SerializeField] public WeaponItemData currentWeapon;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private WeaponItemData testWeapon;

    private BoxCollider2D[] attackColliders;
    private Sprite[] attackSpriteArray;
    private SpriteRenderer playerSprite;
    private Animator attackAnimator;
    private WeaponAnimationHandler animationHandler;
    private Rigidbody2D rb;
    public PlayerInputManger playerInput;
    public GameObject[] slashObj;

    public SpriteRenderer weaponSpriteRenderer;
    public int WeaponTypeIndex { get; private set; }
    public int CurrentAttackSpriteNumber { get; private set; }
    public bool IsAttackActive { get; private set; }
    public event Action OnEnter;
    public event Action OnExit;

    private int facingDirection;
    private int currentAttack;
    private bool hitStop;


    private void Awake()
    {
        InitializeComponents();
        SetWeapon(testWeapon);
        hitStop = false;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void InitializeComponents()
    {
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        if (attackColliders == null || attackColliders.Length == 0)
            Debug.LogError("No BoxCollider2D found in children!");

        if (weaponSpriteRenderer == null)
            Debug.LogError("Weapon SpriteRenderer is missing!");

        playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite == null)
            Debug.LogError("Player SpriteRenderer is missing!");

        animationHandler = GetComponent<WeaponAnimationHandler>();
        if (animationHandler == null)
            Debug.LogError("WeaponAnimationHandler is missing!");

        attackAnimator = GetComponentInChildren<Animator>();
        if (attackAnimator == null)
            Debug.LogError("AttackAnimator is missing!");
    }


    public void SetWeapon(WeaponItemData newWeaponData)
    {
        if(currentWeapon != null)
        {
          inventory.AddItem(currentWeapon, 1);
        }

        currentWeapon = newWeaponData;
        WeaponTypeIndex = (int)newWeaponData.WeaponType;
        attackSpriteArray = currentWeapon.WeaponAttackSprites[facingDirection].AttackSprite;
    }

    public void Activate(int newFacingDirection)
    {
        if (IsAttackActive) return;
        
        IsAttackActive = true;
        weaponSpriteRenderer.enabled = true;
        facingDirection = newFacingDirection;
        CurrentAttackSpriteNumber = 0;
        attackAnimator.Play(GetAnimationName(currentWeapon, facingDirection, currentAttack));
        OnEnter?.Invoke();
        currentAttack++;
      
    }

    public void Deactivate()
    {
        weaponSpriteRenderer.enabled = false;
        facingDirection = -1;
        IsAttackActive = false;
        OnExit?.Invoke();
    }

    public void EnableCollider()
    {

            attackColliders[facingDirection].enabled = true;
        slashObj[facingDirection].SetActive(true);
        //rb.AddForce(new Vector2(playerInput.normInputX,playerInput.normInputY).normalized *10f, ForceMode2D.Impulse);

    }

    public void DisableCollider()
    {
      
            attackColliders[facingDirection].enabled = false;
        slashObj[facingDirection].SetActive(false);
    }

    public void UpdateWeaponSprite(SpriteRenderer spriteRenderer)
    {
        if (CurrentAttackSpriteNumber < attackSpriteArray.Length)
        {
            weaponSpriteRenderer.sprite = attackSpriteArray[CurrentAttackSpriteNumber++];
        }
    }



    private void HandleEnter()
    {
        playerSprite.RegisterSpriteChangeCallback(UpdateWeaponSprite);
    }

    private void HandleExit()
    {
        IsAttackActive = false;
        playerSprite.UnregisterSpriteChangeCallback(UpdateWeaponSprite);
        attackAnimator.Play("EmptyAnim");
    }

    private void OnEnable()
    {
        animationHandler.OnAnimationComplete += Deactivate;
        OnEnter += HandleEnter;
        OnExit += HandleExit;
    }

    private void OnDisable()
    {
        animationHandler.OnAnimationComplete -= Deactivate;
        OnEnter -= HandleEnter;
        OnExit -= HandleExit;
    }

    private string GetAnimationName(WeaponItemData CurrentItemData, int facingDir, int attackIndex)
    {
        return new string(CurrentItemData.WeaponType.ToString() + "_Facing" + facingDirection.ToString() + "_Nr" + (currentAttack + 1).ToString());
    }

    public void StartTimer()
    {
        StartCoroutine(ResetTimer(1));
    }

    public IEnumerator ResetTimer(float time)
    {
        if (currentAttack >= 3) { currentAttack = 0; CurrentAttackSpriteNumber = 0; yield return null; }
        float CachedcurrentAttack = currentAttack;
        yield return new WaitForSeconds(time);
        if (CachedcurrentAttack == currentAttack)
        {
            currentAttack = 0;
            CurrentAttackSpriteNumber = 0;
        }



    }


    public void HitStop()
    {
        if(hitStop) return;
        Time.timeScale = 0f;
        StartCoroutine(ResetTimeScale(0.01f));
    }

    public IEnumerator ResetTimeScale(float time)
    {
        hitStop = true;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        hitStop = false;
    }
}
