using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{

    [System.Serializable]
    private struct CustomerOrder
    {
        public string orderID;
        public CustomerOrderImage orderImage;
    }

    [Header("CUSTOMER ORDER CONFIG")]
    [SerializeField]
    private CanvasGroup customerOrderPanelCanvasGroup;
    [SerializeField]
    private float baseCustomerWaitTime;
    [SerializeField]
    private Slider customerPatienceSlider;
    [SerializeField]
    private Color customerSatisfiedColor;
    [SerializeField]
    private Color customerDisatisfiedColor;
    [SerializeField]
    private CustomerOrder[] currentOrder;

    [HideInInspector]
    public int currentOrderPos;

    private bool hasStartedWaiting = false;
    private bool isSatisfied = true;
    
    private float patienceTimer = 0f;
    private int currentOrderIndex = 0;
    

    [Header("CUSTOMER ANIMATION CONFIG")]
    [SerializeField]
    private float baseWalkTime;

    [Space]

    [SerializeField]
    private SpriteRenderer customerSprite;
    [SerializeField]
    private Sprite[] customerSpriteCollection;
    [SerializeField]
    private int customerActiveSpriteOrder;
    [SerializeField]
    private int customerDoneSpriteOrder;

    private bool isFacingRight = true;

    private void OnEnable()
    {
        InitCustomer();
    }

    public void InitCustomer()
    {
        RandomizeCustomer();
    }

    private void Update()
    {
        if (hasStartedWaiting)
        {
            patienceTimer += Time.deltaTime;
            customerPatienceSlider.value = (baseCustomerWaitTime - patienceTimer) / baseCustomerWaitTime;

            if (patienceTimer >= baseCustomerWaitTime / 2f && isSatisfied)
            {
                isSatisfied = false;
                customerPatienceSlider.fillRect.GetComponent<Image>().color = customerDisatisfiedColor;
            }

            if (patienceTimer >= baseCustomerWaitTime)
            {
                patienceTimer = 0f;
                hasStartedWaiting = false;

                for (int i = 0; i < currentOrder.Length; i++)
                {
                    currentOrder[i].orderID = string.Empty;
                    currentOrder[i].orderImage.ResetOrder();
                    currentOrder[i].orderImage.gameObject.SetActive(false);
                }

                CustomerOrderManager.instance.DeleteCustomerOrder(this);

                EndOrder();

                GameEvent.OnCustomerLeft?.Invoke();
            }

        }
    }


    #region FOOD ORDER

    public void AddFoodOrder(string foodOrderID, FoodItem f, CondimentItem[] condiments, Sprite setPlateImage = null)
    {

        currentOrder[currentOrderIndex].orderID = foodOrderID;
        currentOrder[currentOrderIndex].orderImage.gameObject.SetActive(true);
        currentOrder[currentOrderIndex].orderImage.InitCustomerImage(f, condiments, setPlateImage);

        currentOrderIndex++;

    }

    public void CheckFoodOrder(string foodOrderID)
    {
        for (int i = 0; i < currentOrder.Length; i++)
        {
            if (currentOrder[i].orderID.CompareTo(foodOrderID) == 0)
            {
                currentOrder[i].orderID = string.Empty;
                currentOrder[i].orderImage.ResetOrder();
                currentOrder[i].orderImage.gameObject.SetActive(false);
                currentOrderIndex--;

                break;
            }
        }

        //On Order Complete
        if(currentOrderIndex == 0)
        {
            if(isSatisfied)
            {
                GameEvent.OnCustomerSatisfied?.Invoke();
            }

            GameEvent.OnCustomerServed?.Invoke();

            EndOrder();
        }

    }

    public void SetOrderPanelState(bool state)
    {
        if (state == true)
        {
            customerOrderPanelCanvasGroup.DOFade(1f, 0.125f);
            hasStartedWaiting = true;
        }
        else
        {
            customerOrderPanelCanvasGroup.DOFade(0f, 0.125f);
        }
    }

    private void EndOrder()
    {
        currentOrderIndex = 0;
        patienceTimer = 0f;
        hasStartedWaiting = false;
        SetOrderPanelState(false);
        isSatisfied = true;
        customerPatienceSlider.fillRect.GetComponent<Image>().color = customerSatisfiedColor;
        CustomerOrderManager.instance.OnCompleteCustomerOrder(this);
    }

    #endregion


    #region APPEARANCE AND ANIMATION

    private void RandomizeCustomer()
    {
        int randomSprite = UnityEngine.Random.Range(0, customerSpriteCollection.Length - 1);
        customerSprite.sprite = customerSpriteCollection[randomSprite];
        customerSprite.sortingOrder = customerActiveSpriteOrder;
    }

    public void SetCustomerTargetPosition(Vector3 targetPos, Action OnArrivedAtDestination = null)
    {
        transform.DOMoveX(targetPos.x, baseWalkTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                OnArrivedAtDestination?.Invoke();
            });
    }

    #endregion
}
