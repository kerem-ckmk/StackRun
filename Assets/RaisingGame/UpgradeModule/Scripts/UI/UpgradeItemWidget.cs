using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UpgradeItemWidget : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;
    public Image iconImage;

    [Header("Deactive")]
    public Image deactiveImage;

    public bool IsInitialized { get; private set; }
    public UpgradeItemBase UpgradeItem { get; private set; }

    private Button _button;
    public Button Button
    {
        get
        {
            if (_button == null)
                _button = this.GetComponent<Button>();

            return _button;
        }
    }

    public event Action<UpgradeItemWidget> OnClick;

    public void Initialize(UpgradeItemBase upgradeItem)
    {
        UpgradeItem = upgradeItem;

        titleText.text = UpgradeItem.Name;
        iconImage.sprite = UpgradeItem.Icon;

        Button.onClick.AddListener(ButtonClicked);

        SetActive(true);

        UpdateData();
    }

    private void OnDestroy()
    {
        OnClick = null;
    }

    public void UpdateData()
    {
        levelText.text = $"Level {UpgradeItem.Level}";
        priceText.text = $"{UpgradeItem.Price} <sprite=0>";
    } 

    public void SetActive(bool isActive)
    {
        deactiveImage.enabled = !isActive;
        Button.interactable = isActive;
    }

    private void ButtonClicked()
    {
        OnClick?.Invoke(this);

        GameManager.DoHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.MediumImpact, true);
        GameManager.PlaySound(GameConfigs.Instance.ButtonSound);
    }
}
