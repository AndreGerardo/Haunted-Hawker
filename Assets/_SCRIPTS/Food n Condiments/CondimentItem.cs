using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condiment Item", menuName = "Haunted Hawker/New Condiment Item", order = -1)]
public class CondimentItem : ScriptableObject
{
    public string ID;
    public CondimentModule condimentTablePrefab;
    public Condiment condimentPrefab;
    public int basePrice;
    [Space]
    public Sprite condimentSprite;
    public int spriteOrder;
}
