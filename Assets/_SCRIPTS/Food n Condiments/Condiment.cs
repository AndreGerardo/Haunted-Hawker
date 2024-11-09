using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Condiment : MonoBehaviour
{
    [Header("CONDIMENT CONFIGURATION")]
    [HideInInspector]
    public string ID;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
