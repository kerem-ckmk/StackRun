using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingLevelUpgrade : UpgradeItemBase
{
    public override string Name => "Starting Level";
    public override int Priority => 1;

    protected override Sprite GetIconSprite()
    {
        return GameConfigs.Instance.StartingLevelIcon;
    }

    protected override Sprite GetBackgroundSprite()
    {
        return GameConfigs.Instance.StartingLevelBackground;
    }

    public override int GetPriceForLevel(int level)
    {
        return level * GameConfigs.Instance.StartingLevelUpgradePriceMultiplier;
    }

    public override float GetGameValueForLevel(int level)
    {
        return 1 + (level - 1) * GameConfigs.Instance.StartingLevelMultiplier;
    }
}