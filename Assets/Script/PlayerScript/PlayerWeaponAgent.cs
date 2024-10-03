using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponAgent : MonoBehaviour
{
    [SerializeField]private WeaponItemData CurrentWeapon;
    [SerializeField] private WeaponItemData test;
    private BoxCollider2D[] attackCol = new BoxCollider2D[3];
    private SpriteRenderer weaponSpriteRenderer;
    private int FacingDirection;
    private SpriteRenderer PlayerSprite;
    public int WeaponTypeIndex;
    public int CurrentAttackSpriteNumber;
    


    public void SetWeapon(WeaponItemData newWeaponData)
    {
        CurrentWeapon = newWeaponData;
        WeaponTypeIndex = ((int)newWeaponData.WeaponType);

    }

    public void Activate(int NewFacingDirection)
    {
        weaponSpriteRenderer.enabled = true;
        FacingDirection = NewFacingDirection;
        CurrentAttackSpriteNumber = 0;
        PlayerSprite.RegisterSpriteChangeCallback(ChangeCurrentWeaponSprite);

    }


    public void Deactivate()
    {
        weaponSpriteRenderer.enabled = false;
        FacingDirection = -1;
        PlayerSprite.UnregisterSpriteChangeCallback(ChangeCurrentWeaponSprite);
    }

    public void EnableCol()
    {
        attackCol[FacingDirection].enabled = true;
        
    }

    public void DisableCol() 
    {
        attackCol[FacingDirection].enabled = true;
    }

    public void ChangeCurrentWeaponSprite(SpriteRenderer renderer)
    {
      
        if (CurrentAttackSpriteNumber <= CurrentWeapon.WeaponAttackSprites[FacingDirection].AttackSprite.Length)
        {
            weaponSpriteRenderer.sprite = CurrentWeapon.WeaponAttackSprites[FacingDirection].AttackSprite[CurrentAttackSpriteNumber];
            CurrentAttackSpriteNumber++;
        }
    }

    void Start()
    {
        SetWeapon(test);
        attackCol = GetComponentsInChildren<BoxCollider2D>();
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PlayerSprite = GetComponentInParent<SpriteRenderer>();
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
