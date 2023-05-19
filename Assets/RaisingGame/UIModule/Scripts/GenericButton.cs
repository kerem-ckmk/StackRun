using UnityEngine;
using UnityEngine.UI;

public class GenericButton : Button
{
    private const float PressScaleFactor = 0.92f;

    private Sprite _originalImageSprite;

    private Vector3 _originalScale;
    private float _targetScaleFactor = 1f;
    private float _currentScaleFactor = 1f;

    protected override void Awake()
    {
        base.Awake();

        _originalScale = this.transform.localScale;

        if (interactable && _originalImageSprite == null)
            _originalImageSprite = image.sprite;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!Application.isPlaying)
            return;

        if (_originalImageSprite == null)
            _originalImageSprite = image.sprite;
        
        _targetScaleFactor = state == SelectionState.Pressed ? PressScaleFactor : 1f;

        if (instant)
        {
            UpdateScaleFactor(_targetScaleFactor);
        }

        //////////////////

        if (state == SelectionState.Disabled && spriteState.disabledSprite != null)
        {
            //image.sprite = spriteState.disabledSprite;
        }
        else
        {
            //image.sprite = _originalImageSprite;
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