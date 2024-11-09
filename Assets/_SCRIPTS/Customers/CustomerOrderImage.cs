using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderImage : MonoBehaviour
{
    [Header("FOOD IMAGE CONFIGURATION")]
    [SerializeField]
    private Image plateImage;
    [SerializeField]
    private Image foodImage;
    [SerializeField]
    private Image[] condimentImage;

    public void InitCustomerImage(FoodItem f, CondimentItem[] condiments, Sprite setPlateImage = null)
    {
        SetFood(f, setPlateImage);
        AddCondiment(condiments);
    }

    private void SetFood(FoodItem f, Sprite setPlateImage = null)
    {
        if (setPlateImage != null)
        {
            plateImage.enabled = true;
            plateImage.sprite = setPlateImage;
        }
        else
        {
            plateImage.enabled = false;
        }
        foodImage.sprite = f.cookedSprite;

    }

    private void AddCondiment(CondimentItem[] condiments)
    {
        for (int i = 0; i < condiments.Length; i++)
        {
            condimentImage[i].gameObject.SetActive(true);
            condimentImage[i].sprite = condiments[i].condimentSprite;
        }
    }

    public void ResetOrder()
    {
        for (int i = 0; i < condimentImage.Length; i++) 
        {
            condimentImage[i].gameObject.SetActive(false);
        }
    }

}
