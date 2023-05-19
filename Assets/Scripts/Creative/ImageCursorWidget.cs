using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCursorWidget : MonoBehaviour
{
    [Header("References")]
    public Image cursorImageOpen;
    public Image cursorImageClosed;
    public Image clickImage;

    [Header("Settings")]
    public float clickScale = 0.85f;
    public Color clickColor = Color.white;

    private Sequence _clickImageSequence;

    private void OnEnable()
    {
        clickImage.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        cursorImageOpen.gameObject.SetActive(GameConfigs.Instance.EnableCursorImage && Input.GetMouseButton(0) == false);
        cursorImageClosed.gameObject.SetActive(GameConfigs.Instance.EnableCursorImage && Input.GetMouseButton(0) == true);
        Cursor.visible = !GameConfigs.Instance.EnableCursorImage;

        if (GameConfigs.Instance.EnableCursorImage)
        {
            Vector2 cursorPosition;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)cursorImageOpen.transform.parent, Input.mousePosition, null, out cursorPosition))
            {
                Debug.LogError($"Currency Hud Widget, ScreenPointToLocalPointInRectangle failed for position {Input.mousePosition}");
            }

            cursorImageOpen.rectTransform.anchoredPosition = cursorPosition;
            cursorImageClosed.rectTransform.anchoredPosition = cursorPosition;
            clickImage.rectTransform.anchoredPosition = cursorPosition;


            float targetScale = Input.GetMouseButton(0) ? clickScale : 1f;
            cursorImageOpen.transform.localScale = Vector3.Lerp(cursorImageOpen.transform.localScale, Vector3.one * targetScale, 20f * Time.deltaTime);
            cursorImageClosed.transform.localScale = Vector3.Lerp(cursorImageClosed.transform.localScale, Vector3.one * targetScale, 20f * Time.deltaTime);

            Color targetColor = Input.GetMouseButton(0) ? clickColor : Color.white;
            cursorImageOpen.color = Color.Lerp(cursorImageOpen.color, targetColor, 20f * Time.deltaTime);
            cursorImageClosed.color = Color.Lerp(cursorImageClosed.color, targetColor, 20f * Time.deltaTime);


            if (Input.GetMouseButtonDown(0))
            {
                _clickImageSequence?.Kill();

                _clickImageSequence = DOTween.Sequence();
                _clickImageSequence.AppendCallback(() =>
                {
                    clickImage.gameObject.SetActive(true);
                    clickImage.rectTransform.localScale = Vector3.one;
                });
                _clickImageSequence.Append(clickImage.DOFade(1f, 0f));
                _clickImageSequence.Append(clickImage.DOFade(0f, 0.2f));
                _clickImageSequence.Join(clickImage.transform.DOScale(1.5f, 0.2f));
                _clickImageSequence.AppendCallback(() =>
                {
                    clickImage.gameObject.SetActive(false);
                });
                _clickImageSequence.SetAutoKill(true);
                _clickImageSequence.Play();
            }
        }
    }
}