using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientModule : BaseInteractionModule
{
    [Header("INGREDIENT MODULE CONFIGURATION")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private FoodItem currentFood;

    public override void Interact()
    {
        base.Interact();

        CookingModule cookingModule = cookingStationManager.GetAvailableCookingModule();
        if(cookingModule != null)
        {
            cookingModule.StartCooking(currentFood);
        }

    }

    public void SetFood(FoodItem f)
    {
        currentFood = f;

        spriteRenderer.sprite = f.rawSprite;
        spriteRenderer.sortingOrder = f.spriteOrder;
        spriteRenderer.gameObject.SetActive(true);
    }
}
