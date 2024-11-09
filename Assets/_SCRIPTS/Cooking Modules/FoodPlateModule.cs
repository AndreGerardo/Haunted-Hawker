using DG.Tweening;
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

    [Header("DOUBLE TAP CONFIGURATION")]
    private float lastTapTime = 0f;
    private float doubleTapTime = 0.3f;



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
        condimentObj.basePrice = c.basePrice;
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

        transform.localPosition = Vector3.zero;

        foodOrderID = string.Empty;
        currentFood = null;
        isFoodAvailable = false;
    }

    private void ServeOrder()
    {
        if (!isFoodAvailable) return;

        int totalProfit = 0;

        if (currentFood != null)
        {
            totalProfit = currentFood.basePrice;

            foodOrderID = $"{currentFood.ID}";
            for (int i = 0; i < condimentCollection.Count; i++)
            {
                if (condimentCollection[i].gameObject.activeSelf)
                {
                    foodOrderID += $"_{condimentCollection[i].ID}";
                    totalProfit += condimentCollection[i].basePrice;
                }
            }
        }


        (Customer customer, int orderIndex) = CustomerOrderManager.instance.GetCustomerOrderIfAvailable(foodOrderID);
        if (customer != null)
        {
            transform.DOMove(customer.transform.position, 0.25f)
                .OnComplete(() =>
                {
                    customer.CheckFoodOrder(foodOrderID);
                    CustomerOrderManager.instance.CheckFoodOrderFromList(orderIndex);

                    GameEvent.OnMoneyAdded?.Invoke(totalProfit);

                    ResetPlate();
                });
        }
        else
        {

            if (Time.time - lastTapTime <= doubleTapTime)
            {
                lastTapTime = 0f;
                ResetPlate();
            }
            else
            {
                lastTapTime = Time.time;
            }

            FeedbackBounceAnim();
        }


    }

}
