using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FinishSuccessPanel : UIPanel
{
    [Header("References - UI")]
    public Button continueButton;
    public Image chineseSun;
    public TextMeshProUGUI currencyRewardText;
    public RectTransform continueButtonCurrencyIcon;

    [Header("References - HUD")]
    public CurrencyHudWidget currencyHudWidget;

    private Canvas _rootCanvas;
    private Camera _canvasCamera;

    private bool _isClosing = false;

    private void Awake()
    {
        _rootCanvas = this.GetComponentInParent<Canvas>();
        _canvasCamera = _rootCanvas.worldCamera;

        continueButton.onClick.AddListener(ContinueButtonClicked);
    }

    protected override void OnShowPanel()
    {
        base.OnShowPanel();

        currencyRewardText.text = $"{GameManager.gameplayController.TotalCurrencyReward} <sprite=0>";
        _isClosing = false;
    }

    protected override void OnHidePanel()
    {
        base.OnHidePanel();
    }

    private void ContinueButtonClicked()
    {
        if (_isClosing)
            return;

        _isClosing = true;

        Vector2? popPoint = _canvasCamera.WorldToScreenPoint(continueButtonCurrencyIcon.position);
        GameManager.AddCurrency(GameManager.gameplayController.TotalCurrencyReward, popPoint);

        DOVirtual.DelayedCall(2f, () =>
        {
            GameManager.FullyFinishGameplay();
        });
    }

    private void LateUpdate()
    {
        if (!IsShown)
            return;

        chineseSun.transform.rotation *= Quaternion.AngleAxis(-15f * Time.deltaTime, Vector3.forward);
    }
}
