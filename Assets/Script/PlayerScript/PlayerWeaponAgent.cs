using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponAgent : MonoBehaviour
{
    [SerializeField] private WeaponItemData CurrentWeapon;
    [SerializeField] private WeaponItemData test;
    private BoxCollider2D[] attackCol;
    public SpriteRenderer weaponSpriteRenderer;
    private int FacingDirection;
    private SpriteRenderer PlayerSprite;
    public int WeaponTypeIndex;
    public int CurrentAttackSpriteNumber;
    private Animator AttackAnimator;
    private int currentAttack;
    private WeaponAnimationHandler animationHandler;
    public event Action OnExit;
    private Sprite[] attackSpriteArray;
    public event Action OnEnter;
    public bool isAttackActive;



    public void SetWeapon(WeaponItemData newWeaponData)
    {
        CurrentWeapon = newWeaponData;
        WeaponTypeIndex = ((int)newWeaponData.WeaponType);
        attackSpriteArray = CurrentWeapon.WeaponAttackSprites[FacingDirection].AttackSprite; 

    }

    public void Activate(int NewFacingDirection)
    {
        if(!isAttackActive) {
            isAttackActive = true;
            Debug.Log("l");
            weaponSpriteRenderer.enabled = true;
        FacingDirection = NewFacingDirection;
        CurrentAttackSpriteNumber = 0;
        AttackAnimator.Play(ConvertToAnimationName(CurrentWeapon, FacingDirection, currentAttack));
        OnEnter?.Invoke();
           
        }

    }


    public void Deactivate()
    {
        weaponSpriteRenderer.enabled = false;
        FacingDirection = -1;

        OnExit?.Invoke();
    }

    public void EnableCol()
    {
        attackCol[FacingDirection].enabled = true;
        
    }

    public void DisableCol() 
    {
        attackCol[FacingDirection].enabled = false;
    }

    public void ChangeCurrentWeaponSpriteHandler(SpriteRenderer renderer)
    {
        if (CurrentAttackSpriteNumber <= attackSpriteArray.Length)
        {
            weaponSpriteRenderer.sprite = attackSpriteArray[CurrentAttackSpriteNumber];
            Debug.Log(CurrentAttackSpriteNumber);
            CurrentAttackSpriteNumber++;

        }
    }

    void Awake()
    {
        SetWeapon(test);
        attackCol = GetComponentsInChildren<BoxCollider2D>();
        if (attackCol == null || attackCol.Length == 0)
            Debug.LogError("No BoxCollider2D found in children!");

        //weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (weaponSpriteRenderer == null)
            Debug.LogError("Weapon SpriteRenderer is missing!");

        PlayerSprite = GetComponent<SpriteRenderer>();
        if (PlayerSprite == null)
            Debug.LogError("Player SpriteRenderer is missing!");

        animationHandler = GetComponent<WeaponAnimationHandler>();
        if (animationHandler == null)
            Debug.LogError("WeaponAnimationHandler is missing!");
        AttackAnimator = GetComponentInChildren<Animator>();
        if (AttackAnimator == null)
            Debug.LogError("AttackAnimator is missing!");
    }


    protected virtual void HandleEnter()
    {
        PlayerSprite.RegisterSpriteChangeCallback(ChangeCurrentWeaponSpriteHandler);
    }
    protected virtual void HandleExit()
    {
        isAttackActive = false;
        PlayerSprite.UnregisterSpriteChangeCallback(ChangeCurrentWeaponSpriteHandler);
        AttackAnimator.Play("EmptyAnim");
    }

    public void OnEnable()
    {
        animationHandler.OnAnimationComplete += Deactivate;
        OnEnter += HandleEnter;
        OnExit += HandleExit;

     

    }

    public void OnDisable()
    {
        animationHandler.OnAnimationComplete -= Deactivate;
        OnEnter -= HandleEnter;
        OnExit -= HandleExit;
 
    }

    public string ConvertToAnimationName(WeaponItemData CurrentItemData, int facingDirection, int currentAttack)
    {
        return new string(CurrentItemData.WeaponType.ToString() + "_Facing" + facingDirection.ToString() + "_Nr" + (currentAttack + 1).ToString());
    }
}
