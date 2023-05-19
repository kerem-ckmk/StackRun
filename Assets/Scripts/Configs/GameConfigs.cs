using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfigs", menuName = "Raising Game/Game Configs", order = 1)]
public class GameConfigs : ScriptableObject
{
    public static GameConfigs Instance;

    public bool IsRemoteConfigApplied { get; private set; }

    [Header("Level")]
    public int LevelSkipCountAtRepeat = 0;

    [Header("Economy")]
    public int StartingMoney = 0;

    [Header("Upgrades - Visuals")]
    public Sprite StartingLevelBackground;
    public Sprite StartingLevelIcon;
    public Sprite IncomeBackground;
    public Sprite IncomeIcon;

    [Header("Upgrades - Prices")]
    public int StartingLevelUpgradePriceMultiplier = 100;
    public int IncomeUpgradePriceMultiplier = 100;

    [Header("Upgrades - Functionality")]
    public int StartingLevelMultiplier = 1;
    public float[] IncomeUpgradeMultipliers;

    [Header("Haptic")]
    public float HapticIntervalLimit = 0.15f;

    [Header("Cheats")]
    public int AddCurrencyCheatAmount = 5000;

    [Header("Creative Specific")]
    public bool EnableCursorImage = false;

    [Header("Sounds")]
    public float SoundIntervalLimit = 0.1f;
    public float SoundVolume = 0.5f;
    public float SoundVolumeMultiplier = 1f;
    public AudioClip ButtonSound;

    public void Initialize()
    {
        Debug.Assert(Instance == null);

        Instance = this;
    }

    public void ApplyRemoteConfig()
    {
        IsRemoteConfigApplied = true;
    }
}
