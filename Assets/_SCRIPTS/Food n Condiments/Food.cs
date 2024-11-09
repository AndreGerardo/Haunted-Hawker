using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("FOOD CONFIGURATION")]
    [SerializeField]
    private GameObject rawCollection;
    [SerializeField]
    private GameObject cookedCollection;
    [SerializeField]
    private GameObject burnedCollection;

    public void CookFood()
    {
        cookedCollection.SetActive(true);
        rawCollection.SetActive(false);
    }

    public void BurnFood()
    {
        cookedCollection.SetActive(false);
        burnedCollection.SetActive(true);
    }

    public void ResetFood()
    {
        rawCollection.SetActive(true);
        cookedCollection.SetActive(false);
        //burnedCollection.SetActive(false);
        transform.localScale = Vector3.one;
    }

    public void DeactivateObject()
    {
        ResetFood();
        transform.parent = ObjectPooler.instance.transform;
        gameObject.SetActive(false);
    }
}
