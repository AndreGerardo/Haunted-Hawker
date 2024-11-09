using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingModule : BaseInteractionModule
{
    [Header("COOKING MODULE CONFIGURATION")]
    [SerializeField]
    private SpriteRenderer cookingModuleSprite;
    [SerializeField]
    private Sprite[] cookingStateSprite;

    [Header("FOOD CONFIGURATION")]
    [SerializeField]
    private SpriteRenderer foodSpriteRenderer;

    [HideInInspector]
    public bool isCooking = false;
    [HideInInspector]
    public bool isDoneCooking = false;
    [HideInInspector]
    public bool isBurned = false;

    private FoodItem currentFood;
    private float cookingTime;
    private float burningTime;
    private float timer = 0f;


    public void InitCookingModule(CookingStationManager c, float setCookingTime, float setBurningTime)
    {
        InitModule(c);
        cookingTime = setCookingTime;
        burningTime = setBurningTime;
    }

    public override void Interact()
    {
        base.Interact();

        CollectFood();

    }

    private void Update()
    {
        if (isCooking)
        {
            timer += Time.deltaTime;
            if (timer >= cookingTime)
            {
                DoneCooking();
            }
        }
        else if (isDoneCooking && !isBurned)
        {
            timer += Time.deltaTime;
            if (timer >= burningTime)
            {
                BurnFood();
            }
        }

    }

    public void StartCooking(FoodItem f)
    {
        if (isCooking || isDoneCooking) return;
        isCooking = true;

        currentFood = f;

        foodSpriteRenderer.sprite = f.rawSprite;
        foodSpriteRenderer.sortingOrder = f.spriteOrder;
        foodSpriteRenderer.gameObject.SetActive(true);

        cookingModuleSprite.sprite = cookingStateSprite[1];

        FeedbackBounceAnim();
    }

    private void DoneCooking()
    {
        isCooking = false;
        isDoneCooking = true;
        timer = 0f;

        foodSpriteRenderer.sprite = currentFood.cookedSprite;
    }

    private void BurnFood()
    {
        isBurned = true;
    }

    private void CollectFood()
    {
        if (isCooking) return;

        if (!isDoneCooking) return;

        FoodPlateModule foodPlateModule = cookingStationManager.GetAvailableFoodPlateModule();
        if (foodPlateModule != null)
        {
            isDoneCooking = false;
            cookingModuleSprite.sprite = cookingStateSprite[0];

            foodPlateModule.SetFood(currentFood);

            foodSpriteRenderer.gameObject.SetActive(false);
            currentFood = null;
        }

        

        FeedbackBounceAnim();
    }

}
