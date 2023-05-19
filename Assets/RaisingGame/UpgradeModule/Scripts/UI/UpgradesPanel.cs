using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesPanel : MonoBehaviour
{
    [Header("Upgrade")]
    public LayoutGroup itemsContainer;
    public UpgradeItemWidget upgradeItemWidgetPrefab;

    public bool IsInitialized { get; private set; }
    public UpgradesController UpgradesController { get; private set; }

    private List<UpgradeItemWidget> _upgradeButtons;

    public void Initialize(UpgradesController upgradesController)
    {
        UpgradesController = upgradesController;

        CreateUpgradeButtons();

        IsInitialized = true;
    }

    private void CreateUpgradeButtons()
    {
        itemsContainer.transform.DestroyAllChildren();

        _upgradeButtons = new List<UpgradeItemWidget>();

        foreach (var upgradeItem in UpgradesController.AllUpgradeItems)
        {
            CreateUpgradeButton(upgradeItem, itemsContainer, upgradeItemWidgetPrefab);
        }
    }

    private void CreateUpgradeButton(UpgradeItemBase upgradeItem, LayoutGroup container, UpgradeItemWidget itemWidgetPrefab)
    {
        var upgradeButtonObject = GameObject.Instantiate(itemWidgetPrefab.gameObject, container.transform);
        var upgradeButton = upgradeButtonObject.GetComponent<UpgradeItemWidget>();
        upgradeButton.OnClick += UpgradeButton_OnClick;
        upgradeButton.Initialize(upgradeItem);

        _upgradeButtons.Add(upgradeButton);
    }

    private void UpgradeButton_OnClick(UpgradeItemWidget sender)
    {
        if (GameManager.Instance.TryUpgradeItem(sender.UpgradeItem))
        {
            sender.UpdateData();
            MainPlayerCurrencyUpdated(GameManager.Instance.PlayerCurrencyAmount);
        }
    }

    public void MainPlayerCurrencyUpdated(int currency)
    {
        foreach (var upgradeButton in _upgradeButtons)
        {
            bool enoughFunds = upgradeButton.UpgradeItem.Price <= currency;
            upgradeButton.SetActive(enoughFunds);
        }
    }

    public void UpdateUpgradeItems()
    {
        foreach (var upgradeButton in _upgradeButtons)
        {
            upgradeButton.UpdateData();
        }
    }
}
