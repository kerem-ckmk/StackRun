using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyHudWidget : MonoBehaviour
{
    [Header("References")]
    public Image currencyIconImage;
    public TextMeshProUGUI currencyAmountText;
    public CanvasGroup particleContainer;

    [Header("Settings - Particles")]
    public int groupParticleCount = 20;
    public float particleScale = 0.5f;
    public int maxParticleCurrencyAmount = 0;

    [Header("Settings - Particle Burst")]
    public float particleBurstStartSpeed = 2000f;
    public float particleBurstBrakeLerp = 6f;
    public float particleBurstDuration = 0.5f;

    [Header("Settings - Particle Movement")]
    public float particleMoveAcceleration = 25f; 
    public float maxParticleSpeed = 2000f;

    [Header("Settings - Icon Pop")]
    public float iconPopSpeed = 10f;
    public float iconPopScale = 1.3f;

    [Header("Settings - Text Increase")]
    public float textIncreaseDuration = 0.5f;

    public int CurrencyAmount 
    {
        get { return Mathf.RoundToInt(_currencyAmount); }
    }

    private float _currencyAmount;

    private Canvas _canvas;
    private List<CurrencyHudParticle> _currencyAddAnimationParticlePool;

    private float _currencyIconAnimationFactor;
    private int _currencyAmountTextTarget;
    private int _currencyAmountTextIncrease;

    public event Action OnCurrencyParticleMovementFinished;
    public event Action OnCurrencyAddAnimationFinished;

    private void Awake()
    {
        _canvas = this.GetComponentInParent<Canvas>();

        CreateParticlePool();
    }

    private void OnDisable()
    {
        foreach(var particle in _currencyAddAnimationParticlePool)
        {
            if (particle.IsActive)
            {
                _currencyAmountTextTarget += particle.CarriedCurrency;
                particle.SetActive(false);
            }
        }

        UpdateCurrencyAmount(_currencyAmountTextTarget);
    }

    private void CreateParticlePool()
    {
        _currencyAddAnimationParticlePool = new List<CurrencyHudParticle>(groupParticleCount);
        for (int i = 0; i < groupParticleCount; i++)
        {
            var currencyParticle = CreateCurrencyParticle();
            _currencyAddAnimationParticlePool.Add(currencyParticle);
        }
    }

    private CurrencyHudParticle CreateCurrencyParticle()
    {
        var particleObject = new GameObject("CurrencyHudParticle");
        var particleRectTransform = particleObject.AddComponent<RectTransform>();
        particleRectTransform.SetParent(particleContainer.transform);
        particleRectTransform.anchoredPosition3D = Vector3.zero;
        particleRectTransform.localScale = Vector3.one;

        var particleImage = particleObject.AddComponent<Image>();
        particleImage.sprite = currencyIconImage.sprite;
        particleImage.SetNativeSize();
        particleRectTransform.sizeDelta = particleRectTransform.sizeDelta * particleScale;

        var currencyParticle = new CurrencyHudParticle(particleImage, particleMoveAcceleration, maxParticleSpeed);
        currencyParticle.OnMovementFinished += CurrencyParticle_OnMovementFinished;

        return currencyParticle;
    }

    private int CalculateInactiveParticleCount()
    {
        int inactiveParticleCount = 0;
        foreach (var particle in _currencyAddAnimationParticlePool)
        {
            if (particle.IsActive == false)
                inactiveParticleCount += 1;
        }

        return inactiveParticleCount;
    }

    private void CurrencyParticle_OnMovementFinished(int carriedCurrency)
    {
        _currencyIconAnimationFactor = 1f;

        _currencyAmountTextTarget += carriedCurrency;
        _currencyAmountTextIncrease = Mathf.CeilToInt((_currencyAmountTextTarget - _currencyAmount) / textIncreaseDuration);

        OnCurrencyParticleMovementFinished?.Invoke();
    }

    public void SetCurrencyAmount(int currencyAmount)
    {
        foreach (var particle in _currencyAddAnimationParticlePool)
        {
            particle.SetActive(false);
        }

        _currencyAmountTextTarget = currencyAmount;

        UpdateCurrencyAmount(currencyAmount);
    }

    private void UpdateCurrencyAmount(float currencyAmount)
    {
        _currencyAmount = currencyAmount;
        currencyAmountText.text = CurrencyAmount.ToString();
    }

    public void AddCurrencyLightMode(int addAmount)
    {
        _currencyIconAnimationFactor = 1f;

        _currencyAmountTextTarget += addAmount;
        _currencyAmountTextIncrease = Mathf.CeilToInt((_currencyAmountTextTarget - _currencyAmount) / textIncreaseDuration);
    }

    public void AddCurrencyWithAnimation(int addAmount, Vector2 popScreenPosition, float extraScale = 1f, int exactParticleCount = 0)
    {
        var destination = Vector2.zero;

        Vector2 startPosition;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)particleContainer.transform, popScreenPosition, _canvas.worldCamera, out startPosition))
        {
            Debug.LogError($"Currency Hud Widget, ScreenPointToLocalPointInRectangle failed for position {popScreenPosition}");
        }

        int requiredParticleCount = groupParticleCount;

        if (maxParticleCurrencyAmount > 0)
        {
            requiredParticleCount = Mathf.CeilToInt(Mathf.Clamp01((float)addAmount / maxParticleCurrencyAmount) * groupParticleCount - Mathf.Epsilon);
        }

        if (exactParticleCount != 0)
            requiredParticleCount = exactParticleCount;

        //Add more particle to pool
        int missingParticleCount = requiredParticleCount - CalculateInactiveParticleCount();
        for (int i = 0; i < missingParticleCount; i++)
        {
            var currencyParticle = CreateCurrencyParticle();
            _currencyAddAnimationParticlePool.Add(currencyParticle);
        }

        int currentParticleIndex = 0;
        int lastCumulativeCarriedCurrency = 0;
        foreach (var particle in _currencyAddAnimationParticlePool)
        {
            if (particle.IsActive)
                continue;

            float currencyProgress = (currentParticleIndex + 1f) / requiredParticleCount;
            int cumulativeCarriedCurrency = Mathf.CeilToInt(Mathf.Lerp(0, addAmount, currencyProgress));
            int carriedCurrency = cumulativeCarriedCurrency - lastCumulativeCarriedCurrency;

            particle.SetLocalScale(extraScale);
            particle.StartAnimation(carriedCurrency, startPosition, destination, particleBurstStartSpeed, particleBurstBrakeLerp, particleBurstDuration);

            lastCumulativeCarriedCurrency = cumulativeCarriedCurrency;
            currentParticleIndex += 1;

            if (currentParticleIndex >= requiredParticleCount)
                break;
        }
    }

    private void Update()
    {
        foreach (var particle in _currencyAddAnimationParticlePool)
        {
            particle.Update();
        }

        UpdateCurrencyIcon();
        UpdateAmountText();
    }

    private void UpdateCurrencyIcon()
    {
        if (Mathf.Approximately(_currencyIconAnimationFactor, 0f) == false)
        {
            currencyIconImage.transform.localScale = Vector3.one * Mathf.Lerp(1f, iconPopScale, _currencyIconAnimationFactor);

            _currencyIconAnimationFactor -= iconPopSpeed * Time.deltaTime;

            if (_currencyIconAnimationFactor < 0f)
                _currencyIconAnimationFactor = 0f;
        }
    }

    private void UpdateAmountText()
    {
        if (_currencyAmount < _currencyAmountTextTarget)
        {
            float newCurrencyAmount = _currencyAmount + _currencyAmountTextIncrease * Time.deltaTime;
            newCurrencyAmount = Mathf.Min(newCurrencyAmount, _currencyAmountTextTarget);

            UpdateCurrencyAmount(newCurrencyAmount);

            if (CurrencyAmount >= _currencyAmountTextTarget)
                CurrencyAddAnimationFinish();
        }
    }

    private void CurrencyAddAnimationFinish()
    {
        OnCurrencyAddAnimationFinished?.Invoke();
    }
}

internal class CurrencyHudParticle
{
    public bool IsActive { get; private set; }

    public Image Image { get; private set; }

    public int CarriedCurrency { get; private set; }

    private bool _preIsBurstActive;
    private bool IsBurstActive { get { return Time.time - _activateTime < _burstDuration; } }

    private float _activateTime;

    private float _burstBrake;
    private float _burstDuration;

    private float _moveAcceleration;
    private float _maxSpeed;

    private Vector2 _velocity;
    private Vector2 _destination;

    private Vector2 _firstDestinationDirection;

    private Vector2 _originalImageSize;

    public event Action<int> OnMovementFinished;

    public CurrencyHudParticle(Image image, float moveAcceleration, float maxSpeed)
    {
        Image = image;
        _moveAcceleration = moveAcceleration;
        _maxSpeed = maxSpeed;
        _originalImageSize = Image.rectTransform.sizeDelta;

        SetActive(false);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        Image.color = active ? Color.white : new Color(1f, 1f, 1f, 0f);
    }

    public void SetLocalScale(float localScale)
    {
        Image.rectTransform.sizeDelta = _originalImageSize * localScale;
    }

    public void StartAnimation(int carriedCurrency, Vector2 startPosition, Vector2 destination, float burstStartSpeed, float burstBrake, float burstDuration)
    {
        CarriedCurrency = carriedCurrency;

        _burstBrake = burstBrake;
        _burstDuration = burstDuration;

        _velocity = UnityEngine.Random.onUnitSphere * burstStartSpeed;
        _destination = destination;

        _activateTime = Time.time;
        _preIsBurstActive = true;

        Image.rectTransform.anchoredPosition = startPosition;

        SetActive(true);
    }

    public void Update()
    {
        if (!IsActive)
            return;

        var rectTransform = Image.rectTransform;

        if (IsBurstActive)
        {
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, _burstBrake * Time.deltaTime);
        }
        else
        {
            if (_preIsBurstActive)
            {
                BurstFinished();
            }

            var destinationDirection = (_destination - rectTransform.anchoredPosition).normalized;
            _velocity += destinationDirection * _moveAcceleration * Time.deltaTime;
            _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);
        }
        
        rectTransform.anchoredPosition = rectTransform.anchoredPosition + _velocity * Time.deltaTime;

        var newDestinationDirection = (_destination - rectTransform.anchoredPosition).normalized;
        if (Vector2.Dot(newDestinationDirection, _firstDestinationDirection) < 0)
        {
            MovementFinished();
        }
    }

    private void BurstFinished()
    {
        _preIsBurstActive = false;

        _firstDestinationDirection = (_destination - Image.rectTransform.anchoredPosition).normalized;
    }

    private void MovementFinished()
    {
        SetActive(false);

        OnMovementFinished?.Invoke(CarriedCurrency);
    }
}