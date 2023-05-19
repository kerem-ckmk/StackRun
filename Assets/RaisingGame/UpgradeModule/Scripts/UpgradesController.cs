using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradesController
{
    public List<UpgradeItemBase> AllUpgradeItems = new List<UpgradeItemBase>();

    public event Action<UpgradeItemBase, int> OnItemUpgraded;

    public UpgradesController()
    {
        CreateUpgradeItems();
    }

    private void CreateUpgradeItems()
    {
        AllUpgradeItems.Clear();

        var upgradeItemClassTypes = ReflectionUtilities.GetInheritedClassTypes(typeof(UpgradeItemBase));

        foreach(var upgradeItemClassType in upgradeItemClassTypes)
        {
            var upgradeItem = ReflectionUtilities.CreateInstanceFromTypeName<UpgradeItemBase>(upgradeItemClassType.FullName);
            upgradeItem.OnUpgraded += UpgradeItem_OnUpgraded;

            AllUpgradeItems.Add(upgradeItem);
        }

        AllUpgradeItems.Sort((x, y) => x.Priority.CompareTo(y.Priority));
    }
    
    public T GetUpgradeItem<T>() where T : UpgradeItemBase
    {
        foreach(var upgradeItem in AllUpgradeItems)
        {
            if (upgradeItem is T)
            {
                return (T)upgradeItem;
            }
        }

        return null;
    }

    public void ResetSaveData()
    {
        foreach (var upgradeItem in AllUpgradeItems)
        {
            upgradeItem.ResetSaveData();
        }
    }

    private void UpgradeItem_OnUpgraded(UpgradeItemBase sender, int level)
    {
        OnItemUpgraded?.Invoke(sender, level);
    }
}
