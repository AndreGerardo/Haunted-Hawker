using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GAME CONFIGURATION")]
    [SerializeField]
    private int moneyEarned = 0;
    [SerializeField]
    private int targetMoneyRevenue;
    
    [Space]
    
    [SerializeField]
    private float baseCustomerSatisfactionLevel;
    [SerializeField]
    private float customerSatisfiedIncrement;
    [SerializeField]
    private float customerLeftDecrement;
    

    [Space]

    [SerializeField]
    private float comboBufferTime;
    [SerializeField]
    private int currentComboMultiplier;

    [Space]

    [SerializeField]
    private int customerServed;
    [SerializeField]
    private int targetCustomerServed;

    private float currentComboBufferTimer = 0f;
    private bool isComboStarted = false;
    private float currentCustomerSatisfaction;


    private void Awake()
    {
        GameEvent.OnMoneyAdded += OnMoneyAdded;
        GameEvent.OnCustomerServed += OnCustomerServed;
        GameEvent.OnCustomerSatisfied += OnCustomerSatisfied;
        GameEvent.OnCustomerLeft += OnCustomerLeft;
    }

    private void OnDestroy()
    {
        GameEvent.OnMoneyAdded -= OnMoneyAdded;
        GameEvent.OnCustomerServed -= OnCustomerServed;
        GameEvent.OnCustomerSatisfied -= OnCustomerSatisfied;
        GameEvent.OnCustomerLeft -= OnCustomerLeft;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (isComboStarted)
        {
            currentComboBufferTimer -= Time.deltaTime;

            MainHUDEvent.SetComboMultiplierTime?.Invoke(currentComboBufferTimer, comboBufferTime);

            if (currentComboBufferTimer <= 0f)
            {
                currentComboMultiplier = 0;
                isComboStarted = false;

                MainHUDEvent.SetComboMultiplier?.Invoke(currentComboMultiplier);

            }
        }

        if(currentCustomerSatisfaction <= 0f)
        {
            EndGame();
        }
    }


    public void StartGame()
    {
        currentComboBufferTimer = comboBufferTime;
        currentCustomerSatisfaction = baseCustomerSatisfactionLevel;
        currentComboMultiplier = 0;
        customerServed = 0;

        MainHUDEvent.SetMoney?.Invoke(moneyEarned, targetMoneyRevenue);
        MainHUDEvent.SetCustomerServed?.Invoke(customerServed, targetCustomerServed);
        MainHUDEvent.SetSatisfaction?.Invoke(currentCustomerSatisfaction);

        DOVirtual.DelayedCall(1f, () =>
        {
            CustomerOrderManager.instance.EnableCustomerSpawner();

            GameEvent.OnGameStart?.Invoke();
        });
        
    }

    public void EndGame()
    {
        GameEvent.OnGameOver?.Invoke();
    }


    private void OnMoneyAdded(int money)
    {
        moneyEarned += money + currentComboMultiplier;

        MainHUDEvent.SetMoney?.Invoke(moneyEarned, targetMoneyRevenue);
    }

    private void OnCustomerServed()
    {
        currentComboMultiplier++;
        currentComboBufferTimer = comboBufferTime;
        isComboStarted = true;

        customerServed++;
        MainHUDEvent.SetComboMultiplier?.Invoke(currentComboMultiplier);
        MainHUDEvent.SetCustomerServed?.Invoke(customerServed, targetCustomerServed);
    }

    private void OnCustomerSatisfied()
    {
        currentCustomerSatisfaction += customerSatisfiedIncrement;

        if (currentCustomerSatisfaction > baseCustomerSatisfactionLevel)
        {
            currentCustomerSatisfaction = baseCustomerSatisfactionLevel;
        }

        MainHUDEvent.SetSatisfaction?.Invoke(currentCustomerSatisfaction);


    }
    
    private void OnCustomerLeft()
    {
        currentCustomerSatisfaction -= customerLeftDecrement;

        if (currentCustomerSatisfaction < 0f)
        {
            currentCustomerSatisfaction = 0f;
        }

        MainHUDEvent.SetSatisfaction?.Invoke(currentCustomerSatisfaction);

    }

}

public static class GameEvent
{
    public static Action OnGameStart;
    public static Action OnGameOver;

    public static Action<int> OnMoneyAdded;
    public static Action OnCustomerServed;
    public static Action OnCustomerSatisfied;
    public static Action OnCustomerLeft;
}
