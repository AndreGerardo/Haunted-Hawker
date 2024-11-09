using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondimentModule : BaseInteractionModule
{
    [Header("CONDIMENT MODULE CONFIGURATION")]
    private CondimentItem currentCondimentItem;

    public void InitCondimentModule(CookingStationManager c, CondimentItem setCondimentItem)
    {
        InitModule(c);
        currentCondimentItem = setCondimentItem;
    }

    public override void Interact()
    {
        base.Interact();

        AddCondiment();
    }

    private void AddCondiment()
    {
        FoodPlateModule foodPlateModule = cookingStationManager.GetAvailableCondimentPlate(currentCondimentItem.ID);
        if (foodPlateModule != null)
        {
            foodPlateModule.AddCondiment(currentCondimentItem.ID);            

        }

        FeedbackBounceAnim();
    }
}
