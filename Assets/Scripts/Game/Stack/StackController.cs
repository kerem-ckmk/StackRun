using UnityEngine;

public class StackController : MonoBehaviour
{
    public Transform stackVisual;
    public bool IsInitialized { get; private set; }

    private PositionStatus _currentPositionStatus;

    public void Initialize(float positionZ, float scaleX, float scaleZ, PositionStatus positionStatus = PositionStatus.None)
    {
        _currentPositionStatus = positionStatus;
        transform.SetPositionZ(positionZ);
        stackVisual.localScale = new Vector3(scaleX, 1f, scaleZ);
        ChoosePosition();
        IsInitialized = true;
    }

    public void ChoosePosition()
    {
        if (_currentPositionStatus == PositionStatus.None)
        {
            int randomNumber = Random.Range(0, 10);
            _currentPositionStatus = randomNumber % 2 == 0 ? PositionStatus.Left : PositionStatus.Right;
        }

        float newXPosition = _currentPositionStatus == PositionStatus.Left ? -GameConfigs.Instance.DistanceCenter : GameConfigs.Instance.DistanceCenter;
        transform.SetPositionX(newXPosition);
    }


    private void Update()
    {
        if (!IsInitialized)
            return;

        
    }
    public enum PositionStatus
    {
        None,
        Left,
        Right
    }
}

