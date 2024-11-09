using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum OrderDifficulty
{
    EASY,
    MEDIUM,
    HARD
}

public class CustomerOrderManager : MonoBehaviour
{
    public static CustomerOrderManager instance;

    [System.Serializable]
    private struct CustomerOrder
    {
        public string orderID;
        public Customer customer;
    }

    [System.Serializable]
    private struct CustomerOrderPosition
    {
        public Transform orderPos;
        public bool isAvailable;
    }

    [Header("CUSTOMER ORDER CONFIGURATION")]
    [SerializeField]
    private CookingStationManager[] cookingStationManagers;
    [SerializeField]
    private Transform[] customerOutsidePos;
    [SerializeField]
    private CustomerOrderPosition[] customerOrderPos;
    [SerializeField]
    private List<CustomerOrder> customerOrderList = new List<CustomerOrder>();

    private int customerPoolIndex;
    private int currentCustomerInQueue = 0;

    [Header("CUSTOMER GENERATION CONFIGURATION")]
    [SerializeField]
    private Customer customerPrefab;
    [SerializeField]
    private float customerSpawnRate;

    private float spawnTimer = 0f;


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
    }


    private void Start()
    {
        InitCustomer();
    }

    private void InitCustomer()
    {
        customerPoolIndex = ObjectPooler.instance.AddObject(customerPrefab.gameObject, 6);
    }

    private void Update()
    {

        //DEBUG
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateFoodOrder(OrderDifficulty.EASY);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateFoodOrder(OrderDifficulty.MEDIUM);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GenerateFoodOrder(OrderDifficulty.HARD);
        }
    }

    #region CUSTOMER ORDER

    public (Customer, int) GetCustomerOrderIfAvailable(string foodOrderID)
    {
        if (customerOrderList.Count <= 0) return (null, -1);

        for (int i = 0; i < customerOrderList.Count; i++)
        {
            if (customerOrderList[i].orderID.CompareTo(foodOrderID) == 0)
            {
                return (customerOrderList[i].customer, i);
            }
        }

        return (null, -1);

        //return customerOrderList.FirstOrDefault(order => order.orderID.CompareTo(foodOrderID) == 0).customer;

    }

    public void CheckFoodOrderFromList(int index)
    {
        customerOrderList.RemoveAt(index);
    }

    public void GenerateFoodOrder(OrderDifficulty orderDifficulty)
    {
        if (currentCustomerInQueue >= customerOrderPos.Length) return;

        int randomOrderAmount = UnityEngine.Random.Range(1, 3);

        int orderPosIndex = -1;
        for (int i = 0; i < customerOrderPos.Length; i++)
        {
            if (customerOrderPos[i].isAvailable)
            {
                orderPosIndex = i;
                customerOrderPos[i].isAvailable = false;
                break;
            }
        }

        Customer customerObj = ObjectPooler.instance.GetPooledObject(customerPoolIndex).GetComponent<Customer>();
        customerObj.transform.position = customerOutsidePos[UnityEngine.Random.Range(0, customerOutsidePos.Length - 1)].position;
        customerObj.gameObject.SetActive(true);

        customerObj.SetCustomerTargetPosition(customerOrderPos[orderPosIndex].orderPos.position, () => customerObj.SetOrderPanelState(true));
        customerObj.currentOrderPos = orderPosIndex;

        currentCustomerInQueue++;

        for (int i = 0; i < randomOrderAmount; i++)
        {
            int randomCookingStation = UnityEngine.Random.Range(0, cookingStationManagers.Length - 1);

            (FoodItem generatedFood, CondimentItem[] generatedCondiments) = cookingStationManagers[randomCookingStation].GetRandomFoodItem(orderDifficulty);

            string foodOrderID = $"{generatedFood.ID}";
            for (int j = 0; j < generatedCondiments.Length; j++)
            {
                foodOrderID += $"_{generatedCondiments[j].ID}";
            }

            customerObj.AddFoodOrder(foodOrderID, generatedFood, generatedCondiments, cookingStationManagers[randomCookingStation].foodPlateSprite);

            CustomerOrder customerOrderItem = new CustomerOrder();
            customerOrderItem.orderID = foodOrderID;
            customerOrderItem.customer = customerObj;
            customerOrderList.Add(customerOrderItem);

        }

    }

    public void OnCompleteCustomerOrder(Customer customer)
    {
        customerOrderPos[customer.currentOrderPos].isAvailable = true;
        customer.SetCustomerTargetPosition(customerOutsidePos[UnityEngine.Random.Range(0, customerOutsidePos.Length - 1)].position, () =>
        {
            customer.gameObject.SetActive(false);
        });
        currentCustomerInQueue--;
    }

    #endregion
}
