using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookingStationManager : MonoBehaviour
{
    [System.Serializable]
    private struct IngredientContainer
    {
        public IngredientModule ingredientModule;
        public FoodItem foodItem;
    }

    [Header("INGREDIENTS CONFIG")]
    [SerializeField]
    private IngredientContainer[] ingredientCollection;


    [System.Serializable]
    private struct CondimentContainer
    {
        public Transform condimentPos;
        public CondimentItem condimentItem;
    }

    [Header("INGREDIENTS CONFIG")]
    [SerializeField]
    private CondimentContainer[] condimentCollection;

    [Header("COOKING CONFIG")]
    [SerializeField]
    private float cookingModuleProcessingTime;
    [SerializeField]
    private float cookingModuleBurningTime;
    [SerializeField]
    private List<CookingModule> cookingModuleCollection = new List<CookingModule>();

    [Header("FOOD PLATE MODULE CONFIG")]
    public Sprite foodPlateSprite;
    [SerializeField]
    private List<FoodPlateModule> foodPlateModuleCollection = new List<FoodPlateModule>();


    private void Start()
    {
        InitIngredients();
        InitCondiments();
        InitCookingModule();
        InitFoodPlateModule();
    }

    #region INGREDIENTS MODULE

    private void InitIngredients()
    {
        for (int i = 0; i < ingredientCollection.Length; i++)
        {
            ingredientCollection[i].ingredientModule.InitModule(this);
            ingredientCollection[i].ingredientModule.SetFood(ingredientCollection[i].foodItem);
        }
    }

    #endregion

    #region CONDIMENT MODULE
    private void InitCondiments()
    {
        for (int i = 0; i < condimentCollection.Length; i++)
        {
            CondimentModule condimentModule = Instantiate(condimentCollection[i].condimentItem.condimentTablePrefab, condimentCollection[i].condimentPos);
            condimentModule.transform.localPosition = Vector3.zero; 
            condimentModule.InitCondimentModule(this, condimentCollection[i].condimentItem);
        }
    }

    public FoodPlateModule GetAvailableCondimentPlate(string condimentID)
    {
        return foodPlateModuleCollection.FirstOrDefault(module => module.CheckCondiment(condimentID) == false);
    }

    #endregion

    #region COOKING MODULE

    private void InitCookingModule()
    {
        for(int i = 0; i < cookingModuleCollection.Count; i++)
        {
            cookingModuleCollection[i].InitCookingModule(this, cookingModuleProcessingTime, cookingModuleBurningTime);
        }
    }

    public CookingModule GetAvailableCookingModule()
    {
        return cookingModuleCollection.FirstOrDefault(module => module.isCooking == false && module.isDoneCooking == false);
    }

    #endregion

    #region FOOD PLATE MODULE

    private void InitFoodPlateModule()
    {
        for (int i = 0; i < foodPlateModuleCollection.Count; i++)
        {
            foodPlateModuleCollection[i].InitModule(this);

            for (int j = 0; j < condimentCollection.Length; j++)
            {
                foodPlateModuleCollection[i].SetCondiment(condimentCollection[j].condimentItem);
            }

        }
    }

    public FoodPlateModule GetAvailableFoodPlateModule()
    {
        return foodPlateModuleCollection.FirstOrDefault(module => module.isFoodAvailable == false);
    }

    #endregion

    #region GET AVAILABLE FOOD

    public (FoodItem, CondimentItem[]) GetRandomFoodItem(OrderDifficulty orderDifficulty)
    {
        int rdm = Random.Range(0, ingredientCollection.Length - 1);
        List<CondimentItem> condiments = new List<CondimentItem>();
        int rdmCondiment;

        switch (orderDifficulty)
        {

            case OrderDifficulty.EASY:


                break;
            case OrderDifficulty.MEDIUM:

                rdmCondiment = Random.Range(0, condimentCollection.Length - 1);
                condiments.Add(condimentCollection[rdmCondiment].condimentItem);

                break;
            case OrderDifficulty.HARD:

                for (int i = 0; i < condimentCollection.Length; i++)
                {
                    condiments.Add(condimentCollection[i].condimentItem);
                }

                break;
        }

        return (ingredientCollection[rdm].foodItem, condiments.ToArray());
    }

    #endregion

}
