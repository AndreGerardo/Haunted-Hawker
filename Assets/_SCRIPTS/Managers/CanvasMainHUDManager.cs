using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CanvasMainHUDManager : MonoBehaviour
{
    [Header("MAIN HUD REFERENCE")]
    [SerializeField]
    private TMP_Text satisfactionText;
    [SerializeField]
    private TMP_Text customerServedText;
    [SerializeField]
    private TMP_Text moneyText;
    [SerializeField]
    private TMP_Text comboMultiplierText;
    [SerializeField]
    private Slider comboTimerSlider;

    [Header("STARTING INSTRUCTION REFERENCE")]
    [SerializeField]
    private CanvasGroup instructionCanvasGroup;
    [SerializeField]
    private float instructionDuration;

    [Header("GAME OVER REFERENCE")]
    [SerializeField]
    private CanvasGroup gameOverCanvasGroup;
    [SerializeField]
    private TMP_Text gameOverCustomerServedText;
    [SerializeField]
    private TMP_Text gameOverMoneyText;
    [SerializeField]
    private Button buttonExit;
    [SerializeField]
    private Button buttonRetry;

    private void Awake()
    {
        MainHUDEvent.SetMoney += SetMoney;
        MainHUDEvent.SetCustomerServed += SetCustomerServed;
        MainHUDEvent.SetComboMultiplier += SetComboMultiplier;
        MainHUDEvent.SetComboMultiplierTime += SetComboMultiplierTime;
        MainHUDEvent.SetSatisfaction += SetSatisfaction;

        GameEvent.OnGameStart += PlayStartInstruction;
        GameEvent.OnGameOver += PlayGameOverSequence;
    }

    private void OnDestroy()
    {
        MainHUDEvent.SetMoney -= SetMoney;
        MainHUDEvent.SetCustomerServed -= SetCustomerServed;
        MainHUDEvent.SetComboMultiplier -= SetComboMultiplier;
        MainHUDEvent.SetComboMultiplierTime -= SetComboMultiplierTime;
        MainHUDEvent.SetSatisfaction -= SetSatisfaction;

        GameEvent.OnGameStart -= PlayStartInstruction;
        GameEvent.OnGameOver -= PlayGameOverSequence;
    }

    #region STARTING INSTRUCTION

    private void PlayStartInstruction()
    {
        instructionCanvasGroup.DOFade(1f, 0.5f);
        instructionCanvasGroup.DOFade(0f, 0.5f)
            .SetDelay(instructionDuration);
    }

    #endregion


    #region MAIN HUD

    private void SetMoney(int money, int targetMoney)
    {
        moneyText.SetText($"{money} / {targetMoney}");
    }

    private void SetCustomerServed(int customerServed, int targetCustomerServed)
    {
        customerServedText.SetText($"{customerServed} / {targetCustomerServed}");
    }

    private void SetComboMultiplier(int comboMultiplier)
    {
        comboMultiplierText.SetText($"{comboMultiplier}");
    }

    private void SetComboMultiplierTime(float comboMultiplierTime, float comboMulitplierBufferTime)
    {
        comboTimerSlider.value = comboMultiplierTime / comboMulitplierBufferTime;
    }

    private void SetSatisfaction(float satisfaction)
    {
        satisfactionText.SetText($"{Mathf.RoundToInt(satisfaction)}%");
    }

    #endregion


    #region GAME OVER

    private void PlayGameOverSequence()
    {
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        gameOverCustomerServedText.SetText(gameOverCustomerServedText.text);
        gameOverMoneyText.SetText(moneyText.text);

        gameOverCanvasGroup.DOFade(1f, 1f);
    }

    public void ButtonExit()
    {
        ObjectBounceAnim buttonAnim;
        if (buttonExit.TryGetComponent(out buttonAnim))
        {
            buttonAnim.Play(() =>
            {
                LevelManager.instance.LoadLevel(SceneList.MenuScene);
            });
        }
        else
        {
            LevelManager.instance.LoadLevel(SceneList.MenuScene);
        }
    }

    public void ButtonRetry()
    {
        ObjectBounceAnim buttonAnim;
        if (buttonExit.TryGetComponent(out buttonAnim))
        {
            buttonAnim.Play(() =>
            {
                LevelManager.instance.LoadLevel(SceneList.MainScene);
            });
        }
        else
        {
            LevelManager.instance.LoadLevel(SceneList.MainScene);
        }
    }

    #endregion
}

public static class MainHUDEvent
{
    public static Action<int, int> SetMoney;
    public static Action<int, int> SetCustomerServed;
    public static Action<int> SetComboMultiplier;
    public static Action<float, float> SetComboMultiplierTime;
    public static Action<float> SetSatisfaction;

}