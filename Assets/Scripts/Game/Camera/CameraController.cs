using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera finishCamera;
    public Transform finishCameraTarget;
    public bool IsInitialized { get; private set; }

    private bool _finishGame;
    public void Initialize()
    {
        finishCamera.gameObject.SetActive(false);
        IsInitialized = true;
    }

    public void Prepare(float finishTargetZ)
    {
        _finishGame = false;
        finishCameraTarget.transform.eulerAngles = Vector3.zero;
        finishCameraTarget.SetPositionZ(finishTargetZ);

        playerCamera.gameObject.SetActive(true);
        finishCamera.gameObject.SetActive(false);
    }

    public void FinishGame()
    {
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
