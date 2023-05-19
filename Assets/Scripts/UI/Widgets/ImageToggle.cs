using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageToggle : Toggle
{
    private const float PressScaleFactor = 0.92f;

    private Sprite _originalGraphicSprite;
    private Sprite _uncheckedGraphicSprite;

    private Vector3 _originalScale;
    private float _targetScaleFactor = 1f;
    private float _currentScaleFactor = 1f;

    protected override void Awake()
    {
        base.Awake();

        _originalScale = this.transform.localScale;

        if (interactable && _originalGraphicSprite == null)
            _originalGraphicSprite = image.sprite;

        _uncheckedGraphicSprite = spriteState.selectedSprite;
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;

        bool newIsOn = !isOn;

        image.sprite = newIsOn ? spriteState.selectedSprite : _originalGraphicSprite;

        base.OnPointerClick(eventData);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!Application.isPlaying)
            return;

        if (_originalGraphicSprite == null)
            _originalGraphicSprite = image.sprite;

        _targetScaleFactor = state == SelectionState.Pressed ? PressScaleFactor : 1f;

        if (instant)
        {
            UpdateScaleFactor(_targetScaleFactor);
        }

        //////////////////

        if (state == SelectionState.Disabled && spriteState.disabledSprite != null)
        {
            image.sprite = spriteState.disabledSprite;
        }
        else
        {
            image.sprite = isOn ? spriteState.selectedSprite : _originalGraphicSprite;
        }
    }

    protected virtual void Update()
    {
        float scaleFactor = Mathf.Lerp(_currentScaleFactor, _targetScaleFactor, 20f * Time.deltaTime);
        UpdateScaleFactor(scaleFactor);
    }

    private void UpdateScaleFactor(float scaleFactor)
    {
        _currentScaleFactor = scaleFactor;
        this.transform.localScale = _originalScale * _currentScaleFactor;
    }
}