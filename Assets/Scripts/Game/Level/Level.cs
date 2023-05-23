using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform firstStack;
    public FinishController finishController;
    public List<CollectableBase> collectables;
    public bool IsInitialized { get; private set; }

    public event Action<int> OnAddingAmount;
    public void Initialize(float stackCount)
    {
        finishController.Initialize(GameConfigs.Instance.StackScaleX, stackCount);
        firstStack.transform.SetLocalScaleZ(GameConfigs.Instance.StackScaleZ);

        foreach (var collectable in collectables)
        {
            collectable.Initialize();
            collectable.OnAddCollectablePrice += Collectable_OnAddCollectablePrice;
        }
          
        
        IsInitialized = true;
    }

    private void Collectable_OnAddCollectablePrice(int addAmount)
    {
        OnAddingAmount?.Invoke(addAmount);
    }
}
