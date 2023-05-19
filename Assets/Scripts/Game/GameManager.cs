using System;
using UnityEngine;
using Lofelt.NiceVibrations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public GameplayController gameplayController;

    [Header("Config")]
    public GameConfigs gameConfigs;

    public GameState CurrentGameState { get; private set; }

    public AudioSource AudioSource { get; private set; }

    public bool IsLoadingFinished
    {
        get { return CurrentGameState != GameState.None && CurrentGameState != GameState.Loading; }
    }

    public bool IsVibrationEnabled
    {
        get { return PlayerPrefs.GetInt(PlayerPrefKeys.IsVibrationEnabled, 1) == 1; }
        set
        {
            if (value != IsVibrationEnabled)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.IsVibrationEnabled, value ? 1 : 0);
                VibrationSettingChanged(value);
            }
        }
    }

    public int LinearLevelIndex
    {
        get { return PlayerPrefs.GetInt(PlayerPrefKeys.LinearLevelIndex, 0); }
        set { PlayerPrefs.SetInt(PlayerPrefKeys.LinearLevelIndex, value); }
    }

    public int PlayerCurrencyAmount
    {
        get { return PlayerPrefs.GetInt(PlayerPrefKeys.PlayerCurrencyAmount, GameConfigs.Instance.StartingMoney); }
        set { PlayerPrefs.SetInt(PlayerPrefKeys.PlayerCurrencyAmount, value); }
    }

    private float _lastHapticTime;
    private float _lastSoundTime;

    public event Action<GameState /*Old*/, GameState /*New*/> OnGameStateChanged;
    public event Action<int, int, Vector2?> OnCurrencyChanged;

    private void Awake()
    {
        Instance = this;

        AudioSource = this.gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameConfigs.Instance == null)
        {
            gameConfigs = GameObject.Instantiate(gameConfigs);
            gameConfigs.Initialize();
        }
        else
        {
            gameConfigs = GameConfigs.Instance;
        }

        gameplayController.OnGameplayFinished += GameplayController_OnGameplayFinished;

        DG.Tweening.DOTween.SetTweensCapacity(500, 500);

        VibrationSettingChanged(IsVibrationEnabled);

        ChangeCurrentGameState(GameState.Loading);
    }

    private void VibrationSettingChanged(bool enabled)
    {
        HapticController.hapticsEnabled = enabled;
    }

    private void ChangeCurrentGameState(GameState newGameState)
    {
        var oldGameState = CurrentGameState;
        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(oldGameState, CurrentGameState);
    }

    public void InitializeAfterLoading()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 9999;
#else
        Application.targetFrameRate = 60;
#endif
        gameplayController.Initialize();
    }


    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();

        HardRestart();
    }

    public void PrepareGameplay()
    {
        gameplayController.PrepareGameplay(LinearLevelIndex);
        ChangeCurrentGameState(GameState.Menu);
    }

    public void StartGameplay()
    {
        gameplayController.StartGameplay();

        ChangeCurrentGameState(GameState.Gameplay);
    }

    private void GameplayController_OnGameplayFinished(bool success)
    {
        if (success)
        {
            ChangeCurrentGameState(GameState.FinishSuccess);
            DoHaptic(HapticPatterns.PresetType.Success, true);
        }
        else
        {
            ChangeCurrentGameState(GameState.FinishFail);
            DoHaptic(HapticPatterns.PresetType.Failure, true);
        }
    }

    public void FullyFinishGameplay()
    {
        LinearLevelIndex += 1;

        gameplayController.UnloadGameplay();
        PrepareGameplay();
    }

    public void RetryGameplay()
    {
        gameplayController.UnloadGameplay();
        PrepareGameplay();
    }

    public void HardRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void AddCurrency(int currencyAmount, Vector3 worldPosition)
    {
        Vector2? screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        AddCurrency(currencyAmount, screenPos);
    }

    public void AddCurrency(int currencyAmount, Vector2? screenPosition = null)
    {
        if (currencyAmount == 0)
            return;

        PlayerCurrencyAmount += currencyAmount;
        OnCurrencyChanged?.Invoke(PlayerCurrencyAmount, currencyAmount, screenPosition);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddCurrency(GameConfigs.Instance.AddCurrencyCheatAmount);
        }
    }

    public static void DoHaptic(HapticPatterns.PresetType hapticType, bool dominate = false)
    {
        if (Instance == null)
            return;

        if (dominate || Time.time - Instance._lastHapticTime >= Instance.gameConfigs.HapticIntervalLimit)
        {
            HapticPatterns.PlayPreset(hapticType);
            Instance._lastHapticTime = Time.time;
        }
    }

    public static void PlaySound(AudioClip audioClip, float volume = 0.4f, bool dominate = false)
    {
        if (Instance == null)
            return;

        if (dominate || Time.time - Instance._lastSoundTime >= Instance.gameConfigs.SoundIntervalLimit)
        {
            Instance.AudioSource.volume = volume * GameConfigs.Instance.SoundVolumeMultiplier;
            Instance.AudioSource.PlayOneShot(audioClip);
            Instance._lastSoundTime = Time.time;
        }
    }
}

public enum GameState
{
    None,
    Loading,
    Menu,
    Gameplay,
    FinishSuccess,
    FinishFail
}
