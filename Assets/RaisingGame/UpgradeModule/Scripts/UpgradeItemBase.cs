using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class UpgradeItemBase
{
    public abstract string Name { get; }
    public abstract int Priority { get; }

    public Sprite Icon 
    { 
        get { return GetIconSprite(); } 
    }

    public Sprite Background
    {
        get { return GetBackgroundSprite(); }
    }

    public int Price
    {
        get { return GetPriceForLevel(this.Level); }
    }

    public int Level
    {
        get { return PlayerPrefs.GetInt(SavePrefKey, 1); }
        protected set { PlayerPrefs.SetInt(SavePrefKey, value); }
    }

    public float GameValueFloat
    {
        get { return GetGameValueForLevel(this.Level); }
    }

    public int GameValueInt
    {
        get { return Mathf.RoundToInt(GetGameValueForLevel(this.Level)); }
    }

    public string SavePrefKey { get; private set; }


    public Action<UpgradeItemBase, int> OnUpgraded;


    public UpgradeItemBase()
    {
        SavePrefKey = this.GetType().Name;
    }

    protected abstract Sprite GetIconSprite();
    protected abstract Sprite GetBackgroundSprite();

    public abstract int GetPriceForLevel(int level);

    public abstract float GetGameValueForLevel(int level);

    public void ApplyUpgradeOneLevel()
    {
        Level += 1;

        OnUpgraded?.Invoke(this, Level);
    }

    public void ResetSaveData()
    {
        Level = 1;
    }

    protected float GetContinousData(float[] dataArray, int index)
    {
        if (index < dataArray.Length)
        {
            return dataArray[index];
        }

        if (dataArray.Length == 1)
        {
            return dataArray[0];
        }

        int lastIndex = dataArray.Length - 1;
        int preLastIndex = dataArray.Length - 2;

        float latestDiff = dataArray[lastIndex] - dataArray[preLastIndex];

        return dataArray[lastIndex] + latestDiff * (index - lastIndex);
    }

    protected int GetContinousData(int[] dataArray, int index)
    {
        if (index < dataArray.Length)
        {
            return dataArray[index];
        }

        if (dataArray.Length == 1)
        {
            return dataArray[0];
        }

        int lastIndex = dataArray.Length - 1;
        int preLastIndex = dataArray.Length - 2;

        int latestDiff = dataArray[lastIndex] - dataArray[preLastIndex];

        return dataArray[lastIndex] + latestDiff * (index - lastIndex);
    }
}
