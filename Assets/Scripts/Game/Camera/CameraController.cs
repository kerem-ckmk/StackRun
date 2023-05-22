using Cinemachine;
using DG.Tweening;
using System.Drawing.Drawing2D;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineBrain brain;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera finishCamera;
    public Transform finishCameraTarget;
    public bool IsInitialized { get; private set; }

    private CinemachineBlendDefinition _blend;

    private bool _finishGame;
    public void Initialize()
    {
        _blend = brain.m_DefaultBlend;
        finishCamera.gameObject.SetActive(false);
        IsInitialized = true;
    }

    public void Prepare(float finishTargetZ)
    {
        _finishGame = false;

        _blend.m_Time = 0f;
        brain.m_DefaultBlend = _blend;

        finishCameraTarget.transform.eulerAngles = Vector3.zero;
        finishCameraTarget.SetPositionZ(finishTargetZ);

        playerCamera.gameObject.SetActive(true);
        finishCamera.gameObject.SetActive(false);
    }

    public void FinishGame()
    {
        _blend.m_Time = 1f;
        brain.m_DefaultBlend = _blend;

        _finishGame = true;
        playerCamera.gameObject.SetActive(false);
        finishCamera.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (IsInitialized || _finishGame)
            finishCameraTarget.Rotate(Vector3.up * GameConfigs.Instance.CameraRotationSpeed * Time.deltaTime);

    }
}
