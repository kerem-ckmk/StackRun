using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SettingsPopup : UIPanel
{
    [Header("References - UI")]
    public Button closeButton;
    public Button privacyPolicyButton;
    public Toggle vibrationToggle;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        closeButton.onClick.AddListener(CloseButtonClicked);
        privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonClicked);

        vibrationToggle.onValueChanged.AddListener(VibrationToggleValueChanged);
    }

    protected override void OnShowPanel()
    {
        base.OnShowPanel();

        vibrationToggle.isOn = GameManager.IsVibrationEnabled;
        vibrationToggle.Select();
    }

    private void CloseButtonClicked()
    {
        HidePanel();
    }

    private void PrivacyPolicyButtonClicked()
    {
        Application.OpenURL("https://rollicgames.com/privacy");
    }

    private void VibrationToggleValueChanged(bool value)
    {
        GameManager.IsVibrationEnabled = value;
    }
}