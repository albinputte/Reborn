using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingManager : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemData> craftingRecipes;
    public int currentCraftingMangerIndex;
    [SerializeField] public InventorySO inventory;
    [SerializeField] private List<CraftingRecipe> availableRecipes;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private GameObject Button;
    [SerializeField] private SpriteRenderer spriteRen;
    [SerializeField] private Collider2D Col;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private StructureItemBase structureItem;
    [SerializeField] private InteractableType Type;

    public bool WasPlaced;
    private PlayerInputManger input;
    public bool IsInventoryUi;
    

    public InteractableType type { get => Type; set => Type = value; }
    public SoundType CraftSound;
    public void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        input = FindAnyObjectByType<PlayerInputManger>();
        WasPlaced = false;
       
    }
 
    //delegate add here
    public void Start()
    {
        StartCoroutine(PlaceCooldown());
        craftingUI = CraftingUI.Instance;
        availableRecipes = craftingRecipes
             .Where(item => item.IsCraftable)
             .SelectMany(item => item.craftingRecipe)
             .ToList();
   
    
    }
   



    public void CraftItem(CraftingRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            SoundManager.PlaySound(CraftSound);
            foreach (var ingredient in recipe.ingredients)
            {
                
                RemoveItemsFromInventory(ingredient.item, ingredient.quantity);
            }
            inventory.AddItem(recipe.resultItem, recipe.resultQuantity, null);
            Debug.Log("Crafted " + recipe.resultItem.name);
            //delegate add here
        }
        else
        {
            SoundManager.PlaySound(SoundType.Fail_Craft);
            Debug.Log("failed");
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        Dictionary<ItemData, int> inventoryItems = inventory.GetInventoryState()
            .Where(item => !item.Value.IsEmpty)
            .GroupBy(item => item.Value.item)
            .ToDictionary(group => group.Key, group => group.Sum(i => i.Value.quantity));

        return recipe.ingredients.All(ingredient =>
            inventoryItems.ContainsKey(ingredient.item) && inventoryItems[ingredient.item] >= ingredient.quantity);
    }

    private void RemoveItemsFromInventory(ItemData item, int quantity)
    {
        while (inventory.FindItemInInventory(item) != -1 && quantity > 0)
        {
            int indexPos = inventory.FindItemInInventory(item);
            InventoryItem items = inventory.GetSpecificItem(indexPos);

            if (items.quantity <= quantity)
            {
                inventory.RemoveItem(indexPos, items.quantity);
                quantity -= items.quantity;
            }
            else
            {
                inventory.RemoveItem(indexPos, quantity);
                quantity = 0;
            }
        }

    }

    public List<CraftingRecipe> GetAvailableRecipes()
    {
        return availableRecipes;
    }
    public IEnumerator PlaceCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        WasPlaced = true;
    }

  

    public void Interact()
    {
        if (!WasPlaced)
        {
            input.isInteracting = true;
            return;
        }
            
        

        if (!craftingUI.UiIsActive )
        {
            if (TutorialManger.instance.EshouldAppear())
            {
                TutorialManger.instance.EwasInteractedWith(gameObject.name);
            }

            craftingUI.IsInteractingCrafting = true;
            currentCraftingMangerIndex = craftingUI.checkFirstEmptySlotInCraftingManger();
            craftingUI.craftingManager[craftingUI.checkFirstEmptySlotInCraftingManger()] = this;
            craftingUI.ShowCraftinUi();
            if (!InventoryController.Instance.InventoryUiActive)
                InventoryController.Instance.InventoryInput();
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, false);
        }  
        else
        {
            craftingUI.IsInteractingCrafting = false;
            craftingUI.HideCraftingUi();
            if (InventoryController.Instance.InventoryUiActive && !craftingUI.InteractButtonClicked)
                InventoryController.Instance.InventoryInput();
            else
                craftingUI.InteractButtonClicked = false;
            craftingUI.ClearRecipeInformation();
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);
            
        }
 
    }
    public void InventoryCrafting()
    {

        if (!craftingUI.UiIsActive && !craftingUI.IsInteractingCrafting)
        {

            currentCraftingMangerIndex = craftingUI.checkFirstEmptySlotInCraftingManger();
            craftingUI.craftingManager[craftingUI.checkFirstEmptySlotInCraftingManger()] = this;
            craftingUI.ShowCraftinUi();
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, false);
        }
        else if (!craftingUI.IsInteractingCrafting && craftingUI.UiIsActive)
        {

            craftingUI.HideCraftingUi();
            craftingUI.ClearRecipeInformation();
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);
        }
        else if (craftingUI.IsInteractingCrafting && craftingUI.UiIsActive) {
            input.isInteracting = true;
            input.ActionPefromed = true;
            craftingUI.InteractButtonClicked = true;
            craftingUI.IsInteractingCrafting = false;
       
   
        }
    }
    private void OnDisable()
    {
        if (IsInventoryUi)
        {
            craftingUI.HideCraftingUi();
            craftingUI.ClearRecipeInformation();
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);
        }
    }

    public void DestroyCraftingTable()
    {
        Col.enabled = false;
        spriteRen.enabled = false;
        if (craftingUI.UiIsActive)
        {
            craftingUI.HideCraftingUi();
            craftingUI.ClearRecipeInformation();
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);

        }

        GameObject obj = Instantiate(ItemPrefab, transform.position, Quaternion.identity);
        SoundManager.PlaySound(SoundType.InteractBerryBush);
        obj.GetComponent<WorldItem>().SetItem(structureItem, 1);
        
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (rb != null)
            StartCoroutine(ApplyFloatDrop(rb));
   
   

    }
    public void NearPlayer()
    {
        spriteRen.material = NewMaterial;
        if (TutorialManger.instance.EshouldAppear())
        {
            Button.gameObject.SetActive(true);
        }
    }

    public void LeavingPlayer()
    {
        spriteRen.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }
    private IEnumerator ApplyFloatDrop(Rigidbody2D rb)
    {
        if (rb == null) yield break;
        Vector2[] directions =
        {
            Vector2.left,
            Vector2.right

        };
        float[] randomValues = { 0.4f, 0.4f };
        float checkDist = 0.6f;
        Collider2D[] ItemCol;
        if (Physics2D.Raycast(rb.position, Vector2.up, checkDist, LayerMask.GetMask("Cliff")))
        {
            ItemCol = rb.gameObject.GetComponents<Collider2D>();
            foreach (var col in ItemCol)
            {
                if (!col.isTrigger)
                {
                    col.enabled = false;
                }
            }


        }
        int i = 0;
        foreach (var dir in directions)
        {
            if (!Physics2D.Raycast(rb.position, dir, checkDist, LayerMask.GetMask("Cliff")))
            {
                randomValues[i] = dir.x;
            }
            i++;
        }

        Vector2 force = new Vector2(Random.Range(randomValues[0], randomValues[1]), 1.5f).normalized * 4f;
        if (rb == null)
            yield break;
        rb.AddForce(force, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.2f);
        if (rb == null)
            yield break;
        rb.gravityScale = 1f;


        yield return new WaitForSeconds(0.7f);
        if (rb == null)
            yield break;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }

}