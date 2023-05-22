using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfigs", menuName = "Raising Game/Game Configs", order = 1)]
public class GameConfigs : ScriptableObject
{
    public static GameConfigs Instance;

    [Header("Level")]
    public int LevelSkipCountAtRepeat = 0;

    [Header("Economy")]
    public int StartingMoney = 0;

    [Header("Camera")]
    public float CameraRotationSpeed = 20f;

    [Header("Player")]
    public float PlayerMoveSpeed = 10f;

    [Header("Stack")]
    public float StackMoveSpeed = 10f;
    public float DistanceCenter = 5f;
    public float StackScaleX = 1f;
    public float StackScaleZ = 1f;
    public bool RegularMaterial = false;
    public bool RegularPosition = true;

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
}
