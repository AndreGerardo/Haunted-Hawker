using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlateModule : BaseInteractionModule
{
    [Header("FOOD MODULE CONFIGURATION")]
    [SerializeField]
    private SpriteRenderer foodSprite;

    [HideInInspector]
    public bool isFoodAvailable = false;

    private FoodItem currentFood;
    private string foodOrderID;
    private Dictionary<string, SpriteRenderer> condimentDictionary = new Dictionary<string, SpriteRenderer>();
    private List<Condiment> condimentCollection = new List<Condiment>();


    public override void Interact()
    {
        base.Interact();

        ServeOrder();

    }

    public void SetFood(FoodItem f)
    {
        isFoodAvailable = true;

        currentFood = f;

        foodSprite.gameObject.SetActive(true);
        foodSprite.sprite = f.cookedSprite;
        foodSprite.sortingOrder = f.spriteOrder;

        FeedbackBounceAnim();

    }

    public void SetCondiment(CondimentItem c)
    {
        Condiment condimentObj = Instantiate(c.condimentPrefab, transform);
        condimentObj.ID = c.ID;
        condimentCollection.Add(condimentObj);
        condimentDictionary.Add(condimentObj.ID, condimentObj.spriteRenderer);
        condimentObj.gameObject.SetActive(false);
    }

    public bool CheckCondiment(string condimentID)
    {
        return condimentDictionary[condimentID].gameObject.activeSelf;
    }

    public void AddCondiment(string condimentID)
    {
        condimentDictionary[condimentID].gameObject.SetActive(true);

        FeedbackBounceAnim();
    }

    private void ResetPlate()
    {
        foodSprite.gameObject.SetActive(false);

        for (int i = 0; i < condimentCollection.Count; i++)
        {
            condimentCollection[i].gameObject.SetActive(false);
        }

        foodOrderID = string.Empty;
        currentFood = null;
        isFoodAvailable = false;
    }

    private void ServeOrder()
    {
        if (!isFoodAvailable) return;

        foodOrderID = $"{currentFood.ID}";
        for (int i = 0; i < condimentCollection.Count; i++)
        {
            if (condimentCollection[i].gameObject.activeSelf)
            {
                foodOrderID += $"_{condimentCollection[i].ID}";
            }
        }

        (Customer customer, int orderIndex) = CustomerOrderManager.instance.GetCustomerOrderIfAvailable(foodOrderID);
        if (customer != null)
        {
            customer.CheckFoodOrder(foodOrderID);
            CustomerOrderManager.instance.CheckFoodOrderFromList(orderIndex);
            Debug.Log($"Served Order : {foodOrderID}");
            ResetPlate();
        }
        else
        {
            FeedbackBounceAnim();
        }


    }

}
