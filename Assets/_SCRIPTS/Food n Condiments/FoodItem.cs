using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Haunted Hawker/New Food Item", order = -1)]
public class FoodItem : ScriptableObject
{
    public string ID;
    public int basePrice;
    [Space]
    public Sprite cookedSprite;
    public Sprite rawSprite;
    public int spriteOrder;
}
