using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lofelt.NiceVibrations;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;

    [Header("References - Panels")]
    public LoadingPanel loadingPanel;
    public MenuPanel menuPanel;
    public GameplayPanel gameplayPanel;
    public FinishSuccessPanel finishSuccessPanel;
    public FinishFailPanel finishFailPanel;

    [Header("References - Common HUD")]
    public TextMeshProUGUI levelText;
    public CurrencyHudWidget currencyHudWidget;

    private List<UIPanel> allPanels = new List<UIPanel>();

    private void Awake()
    {
        allPanels.Add(loadingPanel);
        allPanels.Add(menuPanel);
        allPanels.Add(gameplayPanel);
        allPanels.Add(finishSuccessPanel);
        allPanels.Add(finishFailPanel);

        HideAllPanels(true);

        gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        gameManager.OnCurrencyChanged += GameManager_OnCurrencyChanged;
        loadingPanel.OnLoadingFinished += LoadingPanel_OnLoadingFinished;

        currencyHudWidget.OnCurrencyParticleMovementFinished += CurrencyHudWidget_OnCurrencyParticleMovementFinished;
    }

    private void LoadingPanel_OnLoadingFinished(bool extended)
    {
        if (!extended)
        {
            loadingPanel.ExtendLoading();
        }
        else if (extended)
        {
            gameManager.InitializeAfterLoading();

            foreach (var panel in allPanels)
            {
                panel.Initialize(gameManager);
            }

            gameManager.PrepareGameplay();
        }
    }

    private void GameManager_OnGameStateChanged(GameState oldGameState, GameState newGameState)
    {
        if (newGameState == GameState.Loading)
        {
            ShowPanel(loadingPanel);
        }
        else if (newGameState == GameState.Menu)
        {
            OnMenuState();
            ShowPanel(menuPanel);
        }
        else if (newGameState == GameState.Gameplay)
        {
            ShowPanel(gameplayPanel);
        }
        else if (newGameState == GameState.FinishSuccess)
        {
            ShowPanel(finishSuccessPanel);
        }
        else if (newGameState == GameState.FinishFail)
        {
            ShowPanel(finishFailPanel);
        }
        else
        {
            HideAllPanels();
        }
    }

    private void HideAllPanels(bool forceHide = false)
    {
        foreach (var panel in allPanels)
        {
            if (panel.IsShown || forceHide)
                panel.HidePanel();
        }
    }

    private void ShowPanel(UIPanel panel)
    {
        HideAllPanels();

        panel.ShowPanel();
    }

    private void OnMenuState()
    {
        levelText.text = $"Level {gameManager.LinearLevelIndex + 1}";
        currencyHudWidget.SetCurrencyAmount(gameManager.PlayerCurrencyAmount);
    }

    private void GameManager_OnCurrencyChanged(int totalCurrency, int changeAmount, Vector2? popScreenPos)
    {
        if (changeAmount > 0 && popScreenPos != null)
        {
            currencyHudWidget.AddCurrencyWithAnimation(changeAmount, popScreenPos.Value);
        }
        else
        {
            currencyHudWidget.SetCurrencyAmount(totalCurrency);
        }
    }

    private void CurrencyHudWidget_OnCurrencyParticleMovementFinished()
    {
        GameManager.DoHaptic(HapticPatterns.PresetType.SoftImpact);
    }
}
