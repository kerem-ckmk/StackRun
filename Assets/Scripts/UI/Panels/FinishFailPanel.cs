using UnityEngine;
using UnityEngine.UI;

public class FinishFailPanel : UIPanel
{
    [Header("References - UI")]
    public Button retryButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(RetryButtonClicked);
    }

    protected override void OnShowPanel()
    {
        base.OnShowPanel();
    }

    protected override void OnHidePanel()
    {
        base.OnHidePanel();
    }

    private void RetryButtonClicked()
    {
        GameManager.RetryGameplay();
    }
}
