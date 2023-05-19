using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class UIUtilities
{
    public static Sequence CreateRewardedButtonSequence(Button rewardedButton)
    {
        if (rewardedButton == null)
            return null;

        float maxRotateAngle = 8f;
        float rotateOneMotionDuration = 0.1f;

        var sequence = DOTween.Sequence();

        sequence.AppendInterval(1f);
        sequence.Append(rewardedButton.transform.DOScale(1.1f, 0.25f));
        sequence.Append(rewardedButton.transform.DORotate(new Vector3(0f, 0f, -maxRotateAngle), rotateOneMotionDuration * 0.5f, RotateMode.Fast));
        sequence.Append(rewardedButton.transform.DORotate(new Vector3(0f, 0f, maxRotateAngle), rotateOneMotionDuration, RotateMode.Fast));
        sequence.Append(rewardedButton.transform.DORotate(new Vector3(0f, 0f, -maxRotateAngle), rotateOneMotionDuration, RotateMode.Fast));
        sequence.Append(rewardedButton.transform.DORotate(new Vector3(0f, 0f, maxRotateAngle), rotateOneMotionDuration, RotateMode.Fast));
        sequence.Append(rewardedButton.transform.DORotate(new Vector3(0f, 0f, 0f), rotateOneMotionDuration * 0.5f, RotateMode.Fast));
        sequence.Append(rewardedButton.transform.DOScale(1f, 0.25f));
        sequence.AppendInterval(1f);

        sequence.SetLoops(-1);
        sequence.SetAutoKill(false);

        return sequence;
    }
}
