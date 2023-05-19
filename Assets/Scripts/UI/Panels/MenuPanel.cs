using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : UIPanel
{
    [Header("References - UI")]
    public Button startButton;
    public Button settingsButton;
    public SettingsPopup settingsPopup;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        settingsPopup.Initialize(GameManager);
        settingsPopup.HidePanel();
    }

    protected override void OnShowPanel()
    {
        base.OnShowPanel();
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
}
