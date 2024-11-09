using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemLibrary : MonoBehaviour
{
    public static FoodItemLibrary instance;

    [Header("FOOD ITEM LIBRARY")]
    [SerializeField]
    private FoodItem[] foodItemCollection;

    private Dictionary<string, int> foodItemLibrary = new Dictionary<string, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitFoodItemLibrary();
    }

    #region FOOD ITEM

    private void InitFoodItemLibrary()
    {
        for (int i = 0; i < foodItemCollection.Length; i++)
        {
            //string foodItemID = foodItemCollection[i].ID;
            //int foodItemIndex = ObjectPooler.instance.AddObject(foodItemCollection[i].foodPrefab.gameObject);

            //if (!foodItemLibrary.ContainsKey(foodItemID))
            //{
            //    foodItemLibrary.Add(foodItemID, foodItemIndex);
            //}

        }
    }

    #endregion


}
