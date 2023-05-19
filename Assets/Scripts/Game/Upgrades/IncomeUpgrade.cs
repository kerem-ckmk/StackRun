using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeUpgrade : UpgradeItemBase
{
    public override string Name => "Income";
    public override int Priority => 3;

    protected override Sprite GetIconSprite()
    {
        return GameConfigs.Instance.IncomeIcon;
    }

    protected override Sprite GetBackgroundSprite()
    {
        return GameConfigs.Instance.IncomeBackground;
    }

    public override int GetPriceForLevel(int level)
    {
        return level * GameConfigs.Instance.IncomeUpgradePriceMultiplier;
    }

    public override float GetGameValueForLevel(int level)
    {
        return GetContinousData(GameConfigs.Instance.IncomeUpgradeMultipliers, level - 1);
    }
}