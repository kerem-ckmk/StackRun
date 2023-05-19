using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : UIPanel
{
    [Header("References - UI")]
    public Button startButton;
    public Button settingsButton;
    public SettingsPopup settingsPopup;
    public UpgradesPanel upgradesPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        upgradesPanel.Initialize(GameManager.Instance.gameplayController.UpgradesController);

        GameManager.OnCurrencyChanged += GameManager_OnCurrencyChanged;

        settingsPopup.Initialize(GameManager);
        settingsPopup.HidePanel();
    }

    protected override void OnShowPanel()
    {
        base.OnShowPanel();

        GameManager_OnCurrencyChanged(GameManager.Instance.PlayerCurrencyAmount, 0, null);

        upgradesPanel.UpdateUpgradeItems();
    }

    protected override void OnHidePanel()
    {
        base.OnHidePanel();
    }

    private void StartButtonClicked()
    {
        GameManager.StartGameplay();
    }

    private void SettingsButtonClicked()
    {
        settingsPopup.ShowPanel();
    }

    private void GameManager_OnCurrencyChanged(int totalCurrency, int changeAmount, Vector2? position)
    {
        upgradesPanel.MainPlayerCurrencyUpdated(totalCurrency);
    }
}
